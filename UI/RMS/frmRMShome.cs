using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Guna.UI2.WinForms;
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
using YamyProject.RMS.Class;

namespace YamyProject
{
    public partial class frmRMShome : Form
    {
        public frmRMShome()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

        }

        private void frmRMShome_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void loadData()
        {


            string qry = @"SELECT MainId,aDate,time, orderType, Total FROM tbl_rmsmain ORDER BY aDate DESC, time DESC; ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvDate);
            lb.Items.Add(dgvtime);
            lb.Items.Add(dgvordertype);
            lb.Items.Add(dgvPrice);

            RMSClass.loadData(qry, guna2DataGridView1, lb);

        }

        private void loadChartData(double dinIn, double takeAway, double delivery)
        {
            // Clear old series if any
            gunaChart1.Datasets.Clear();
            //gunaChart1.Labels = null;

            // Set title (optional)
            //gunaChart1.Legend.Display = true;
            //gunaChart1.Title.Text = "Open Leads";

            var dataset = new Guna.Charts.WinForms.GunaBarDataset();
            dataset.Label = "Orders";
            dataset.DataPoints.Add("Dine In", dinIn);       // sample value
            dataset.DataPoints.Add("Take Away", takeAway);
            dataset.DataPoints.Add("Delivery", delivery);

            // Optional styling
            //dataset.BackgroundColor = new List<System.Drawing.Color>
            //    {
            //        System.Drawing.Color.FromArgb(54, 162, 235),
            //        System.Drawing.Color.FromArgb(255, 206, 86),
            //        System.Drawing.Color.FromArgb(255, 99, 132)
            //    };
            gunaChart1.Datasets.Add(dataset);
            gunaChart1.Update();

        }

        private void gettotal()
        {
            double tot = 0;
            guna2HtmlLabel8.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvPrice"].Value.ToString());

            }
            guna2HtmlLabel8.Text = tot.ToString("N2");
        }
        private int num = 0;
      
        public DataTable LOADTOTAL(string id)
        {
            return DBClass.ExecuteDataTable("SELECT COUNT(orderType) FROM `tbl_rmsmain` WHERE orderType =@1 and status = 'Pending'", DBClass.CreateParameter("@1", id));

        }

        public DataTable loadpaid(string id)
        {
            return DBClass.ExecuteDataTable("SELECT SUM(Total) FROM `tbl_rmsmain` WHERE status =@1  ", DBClass.CreateParameter("@1", id));

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
                sst =   count.ToString();
            }
            else
            {
                // If no result found, show a default message
                sst = "0";
            }
        }

        public void showdata1(string id)
        {
            // Call the LOADTOTAL method to get the DataTable with the count
            DataTable result = loadpaid(id);

            // Check if the DataTable contains any rows
            if (result.Rows.Count > 0)
            {
                // Extract the value from the first row and first column
                var value = result.Rows[0][0];

                // Check if the value is DBNull before converting
                if (value != DBNull.Value)
                {
                    // Convert the value to an integer
                    int count = Convert.ToInt32(value);

                    // Set the label text with the count
                    sst2 = count.ToString();
                }
                else
                {
                    // If the value is DBNull, set a default message or 0
                    sst2 = "0.00";
                }
            }
            else
            {
                // If no result found, show a default message
                sst2 = "0.00";
            }
        }
        public void showdata2(string id)
        {
            // Call the LOADTOTAL method to get the DataTable with the count
            DataTable result = loadpaid(id);

            // Check if the DataTable contains any rows
            if (result.Rows.Count > 0)
            {
                // Extract the value from the first row and first column
                var value = result.Rows[0][0];

                // Check if the value is DBNull before converting
                if (value != DBNull.Value)
                {
                    // Convert the value to an integer
                    int count = Convert.ToInt32(value);

                    // Set the label text with the count
                    sst3 = count.ToString();
                }
                else
                {
                    // If the value is DBNull, set a default message or 0
                    sst3 = "0.00";
                }
            }
            else
            {
                // If no result found, show a default message
                sst2 = "0.00";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCustom)
            {
                timer1.Stop();
                return;
            }
            else
            {
                loadData();
                gettotal();
                //LOADTOTAL(num, "Paid");
                //guna2HtmlLabel12.Text = num.ToString();
                ShowTotalCountInLabel("Din in");
                guna2HtmlLabel6.Text = sst;
                ShowTotalCountInLabel("Take Away");
                guna2HtmlLabel19.Text = sst;
                ShowTotalCountInLabel("Delivery");
                guna2HtmlLabel20.Text = sst;
                Int32 nnn = 0;
                nnn = Convert.ToInt32(guna2HtmlLabel20.Text) + Convert.ToInt32(guna2HtmlLabel19.Text) + Convert.ToInt32(guna2HtmlLabel6.Text);
                guna2HtmlLabel21.Text = nnn.ToString();
                showdata1("Paid");
                guna2HtmlLabel12.Text = sst2;
                showdata1("Complete");
                showdata2("Pending");
                guna2HtmlLabel5.Text = sst2 + sst3;


                loadChartData(Convert.ToDouble(guna2HtmlLabel6.Text), Convert.ToDouble(guna2HtmlLabel19.Text), Convert.ToDouble(guna2HtmlLabel20.Text));
            }
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //
        }

        private void guna2DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //
        }

        bool isCustom = false;

        private void customReport()
        {
            isCustom = true;
        }

        private void cbrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbrole.SelectedIndex == 0)
            {
                isCustom = false;
                customReport();
            }
        }
    }
}
