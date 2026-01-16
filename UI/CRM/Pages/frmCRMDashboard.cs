using DocumentFormat.OpenXml.Drawing;
using Guna.UI2.WinForms;
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

namespace YamyProject.UI.CRM.Pages
{
    public partial class frmCRMDashboard : Form
    {
        public frmCRMDashboard()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }


        public DataTable LOADTOTAL(string id)
        {
            return DBClass.ExecuteDataTable("SELECT COUNT(Stage) FROM `tbl_crmcustomer`", DBClass.CreateParameter("@1", id));

        }
        public DataTable loadpaid(string id)
        {
            return DBClass.ExecuteDataTable("SELECT COUNT(Stage) FROM `tbl_crmcustomer` WHERE openlvl =@1  ", DBClass.CreateParameter("@1", id));

        }
        public string sst = "";
        public string sst2 = "";
        public string sst3 = "";
        public void ShowTotalCountInLabel(string id)
        {
            // Call the LOADTOTAL method to get the DataTable with the count
            DataTable result = LOADTOTAL(id);

            // Check if the DataTable contains any rows
            if (result.Rows.Count > 0)
            {
                // Extract the count from the first row and first column (since it's a single value)
                int count = Convert.ToInt32(result.Rows[0][0]);

                // Set the label text with the count
                sst = count.ToString();
            }
            else
            {
                // If no result found, show a default message
                sst = "0";
            }
        }
        public void ShowTotalCountInLabel2(string id)
        {
            // Call the LOADTOTAL method to get the DataTable with the count
            DataTable result = loadpaid(id);

            // Check if the DataTable contains any rows
            if (result.Rows.Count > 0)
            {
                // Extract the count from the first row and first column (since it's a single value)
                int count = Convert.ToInt32(result.Rows[0][0]);

                // Set the label text with the count
                sst = count.ToString();
            }
            else
            {
                // If no result found, show a default message
                sst = "0";
            }
        }

        private void frmCRMDashboard_Load(object sender, EventArgs e)
        {
            ShowTotalCountInLabel("Din in");
            label2.Text = sst;
            ShowTotalCountInLabel2("Open");
            label3.Text = sst;
            ShowTotalCountInLabel2("Won");
            label5.Text = sst;
            ShowTotalCountInLabel2("Lost");
            label7.Text = sst;
        }

        //private void LoadChartData()
        //{
        //    // 1. إعداد الاتصال بقاعدة البيانات
        //    string connectionString = "server=localhost;user=root;password=1234;database=mydb;";
        //    MySqlConnection conn = new MySqlConnection(connectionString);

        //    try
        //    {
        //        conn.Open();

        //        // 2. تنفيذ الاستعلام
        //        string query = "SELECT month, amount FROM sales ORDER BY id";
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        MySqlDataReader reader = cmd.ExecuteReader();

        //        // 3. تجهيز القيم
        //        var months = new List<string>();
        //        var values = new List<double>();

        //        while (reader.Read())
        //        {
        //            months.Add(reader.GetString("month"));
        //            values.Add(reader.GetDouble("amount"));
        //        }

        //        // 4. إعداد الشارت (Guna Chart)
        //        gunaChart1.Datasets.Clear();
        //        var dataset = new Guna.Charts.WinForms.GunaChartDataset();
        //        dataset.Label = "المبيعات";
        //        dataset.Data = values;

        //        gunaChart1.Datasets.Add(dataset);
        //        gunaChart1.Labels = months;

        //        gunaChart1.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
    }
}
