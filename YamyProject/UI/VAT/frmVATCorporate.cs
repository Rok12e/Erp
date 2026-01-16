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

namespace YamyProject.UI.VAT
{
    public partial class frmVATCorporate : Form
    {
        public frmVATCorporate()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void frmVATCorporate_Load(object sender, EventArgs e)
        {
            DateTime dated = DateTime.Now;
            DateTime startOfYear = new DateTime(dated.Year, 1, 1);
            DateTime endOfYear = new DateTime(dated.Year, 12, 31);
            label126.Text = label128.Text = startOfYear.ToShortDateString();
            label127.Text = label129.Text = endOfYear.ToShortDateString();
            label130.Text = dated.ToShortDateString();
            loadCompany();
            loadData();
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name,trn_no FROM tbl_company"))
            {
                if (reader.Read()) {
                    if (reader["name"] != DBNull.Value)
                    {
                        label15.Text = reader["name"].ToString();
                    }
                    if (reader["trn_no"] != DBNull.Value) {
                        label8.Text = reader["trn_no"].ToString();
                    }
                }
            }
        }
        private void loadData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT id, Name, value, Description FROM tbl_tax WHERE state = 0"))
            {
                if (reader.Read())
                {
                    int vatId = int.Parse(reader["id"].ToString());
                    string name = reader["Name"].ToString();
                    using (MySqlDataReader reader1 = DBClass.ExecuteReader(@"SELECT 
                                                        IFNULL(SUM(s.total),0) AS 'Amount Before Vat',
                                                        IFNULL(SUM(s.vat),0) AS 'Vat Amount',
                                                        IFNULL(SUM(s.net),0) AS 'Total Amount'
                                                    FROM 
                                                        tbl_sales s
                                                    JOIN 
                                                        tbl_customer c ON s.customer_id = c.id
                                                    JOIN 
                                                        tbl_sales_details sd ON sd.sales_id = s.id
                                                    WHERE 
                                                        s.vat > 0
                                                        AND sd.vat=@vatId
                                                    AND s.DATE BETWEEN @datefrom AND @dateTo",
                                                            DBClass.CreateParameter("vatId", vatId),
                                                DBClass.CreateParameter("dateFrom", DateTime.Parse(label128.Text)),
                                                DBClass.CreateParameter("dateTo", DateTime.Parse(label129.Text))
                                                ))
                    {
                        if (reader1.Read())
                        {
                            decimal beforTax = 0, afterTax = 0, taxAmount = 0;
                            beforTax = decimal.Parse(reader1["Amount Before Vat"].ToString());
                            afterTax = decimal.Parse(reader1["Total Amount"].ToString());
                            taxAmount = decimal.Parse(reader1["Vat Amount"].ToString());

                            if (name.Contains("Abu Dhabi"))
                            {
                                label90.Text = taxAmount.ToString("N2");
                                label41.Text = afterTax.ToString("N2");
                                //label27.Text = "";
                            }
                            else if (name.Contains("Dubai"))
                            {
                                label40.Text = taxAmount.ToString("N2");
                                label31.Text = afterTax.ToString("N2");
                                //label89.Text = "";
                            }
                            else if (name.Contains("Sharjah"))
                            {
                                label39.Text = taxAmount.ToString("N2");
                                label33.Text = afterTax.ToString("N2");
                                //label88.Text = "";
                            }
                            else if (name.Contains("Ajman"))
                            {
                                label38.Text = taxAmount.ToString("N2");
                                label32.Text = afterTax.ToString("N2");
                                //label100.Text = "";
                            }
                            else if (name.Contains("Umm Al Quwain"))
                            {
                                label36.Text = taxAmount.ToString("N2");
                                label35.Text = afterTax.ToString("N2");
                                //label99.Text = "";
                            }
                            else if (name.Contains("Rash Al Khaimah"))
                            {
                                label30.Text = taxAmount.ToString("N2");
                                label34.Text = afterTax.ToString("N2");
                                //label93.Text = "";
                            }
                            else if (name.Contains("Fujairah"))
                            {
                                label28.Text = taxAmount.ToString("N2");
                                label37.Text = afterTax.ToString("N2");
                                //label108.Text = "";
                            }
                            else if (name.Contains("tax refund"))
                            {
                                label51.Text = taxAmount.ToString("N2");
                                label49.Text = afterTax.ToString("N2");
                                //label102.Text = "";
                            }
                            else if (name.Contains("reverse accounting"))
                            {
                                label50.Text = taxAmount.ToString("N2");
                                label48.Text = afterTax.ToString("N2");
                                //label101.Text = "";
                            }
                            else if (name.Contains("zero rated"))
                            {
                                //label90.Text = taxAmount.ToString("N2");
                                label52.Text = afterTax.ToString("N2");
                                //label27.Text = "";
                            }
                            else if (name.Contains("Exempt"))
                            {
                                //label90.Text = taxAmount.ToString("N2");
                                label53.Text = afterTax.ToString("N2");
                                //label27.Text = "";
                            }
                            else if (name.Contains("Goods imported into the country"))
                            {
                                label55.Text = taxAmount.ToString("N2");
                                label54.Text = afterTax.ToString("N2");
                                //label27.Text = "";
                            }
                            else if (name.Contains("Settlement of goods imported into the United Arab Emirates"))
                            {
                                label57.Text = taxAmount.ToString("N2");
                                label56.Text = afterTax.ToString("N2");
                                //label27.Text = "";
                            }
                        }
                    }
                }
            }
        }
    }
}
