using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using YamyProject.RMS.Class;
using MessageBox = System.Windows.Forms.MessageBox;

namespace YamyProject.RMS.View
{
    public partial class frmRMSKitchview : Form
    {
        public frmRMSKitchview()
        {
            InitializeComponent();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"Select * from tbl_rmsmain WHERE status = 'Pending' ";
            MySqlCommand cmd1 = new MySqlCommand(qry1 ,RMSClass.conn());
            DataTable dt1 = new DataTable();
            MySqlDataAdapter DA = new MySqlDataAdapter(cmd1);
            DA.Fill(dt1);

            FlowLayoutPanel P1;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                P1 = new FlowLayoutPanel();
                P1.AutoSize = true;
                P1.Width = 230;
                P1.Height = 350;
                P1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                P1.BorderStyle = BorderStyle.FixedSingle;
                P1.Margin = new Padding(10,10,10,10);

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(50, 55, 89);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 125;
                p2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lb1 = new Label();
                lb1.ForeColor = Color.White;
                lb1.Margin = new Padding(10, 10, 3, 0);
                lb1.AutoSize = true;

                Label lb2 = new Label();
                lb2.ForeColor = Color.White;
                lb2.Margin = new Padding(10, 5, 3, 0);
                lb2.AutoSize = true;

                Label lb3 = new Label();
                lb3.ForeColor = Color.White;
                lb3.Margin = new Padding(10, 5, 3, 0);
                lb3.AutoSize = true;

                Label lb4 = new Label();
                lb4.ForeColor = Color.White;
                lb4.Margin = new Padding(10, 5, 30, 10);
                lb4.AutoSize = true;

                lb1.Text = "Table : " + dt1.Rows[i]["tableName"].ToString();
                lb2.Text = "Waiter Name : " + dt1.Rows[i]["waiterName"].ToString();
                lb3.Text = "Order Time : " + dt1.Rows[i]["time"].ToString();
                lb4.Text = "Order Type : " + dt1.Rows[i]["orderType"].ToString();

                p2.Controls.Add(lb1);
                p2.Controls.Add(lb2);
                p2.Controls.Add(lb3);
                p2.Controls.Add(lb4);

              P1.Controls.Add(p2);

                //
                int mid = 0;
                mid = Convert.ToInt32(dt1.Rows[i]["MainID"].ToString());
                string qry2 = @"Select * from tbl_rmsmain m 
                                 inner join tbl_rmsdetails d on m.MainId = d.MainID
                                 inner join tbl_items p on p.id = d.proID 
                                 where m.MainId = "+mid+"";
                MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
                DataTable dt2 = new DataTable();
                MySqlDataAdapter DA2 = new MySqlDataAdapter(cmd2);
                DA2.Fill(dt2);
                for (int j = 0; j < dt2.Rows.Count; j++)
                {

                    Label lb5 = new Label();
                    lb5.ForeColor = Color.Black;
                    lb5.Margin = new Padding(10, 5, 3, 10);
                    lb5.AutoSize = true;
                    int no = j + 1;
                    
                    lb5.Text = "(" + no + ") " + dt2.Rows[j]["name"].ToString() + "-" + dt2.Rows[j]["qty"].ToString();
                    P1.Controls.Add(lb5);

                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new System.Drawing.Size(100, 35);
                b.FillColor = Color.FromArgb(241, 85, 126);
                b.Margin = new Padding(30, 5, 3, 10);
                b.Text = "Complete";
                b.Tag = dt1.Rows[i]["MainId"].ToString();

                b.Click += new EventHandler(b_click);
                P1.Controls.Add(b);

                flowLayoutPanel1.Controls.Add(P1);
            }
        }
        private void b_click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());
            DialogResult result = MessageBox.Show(
                 "Is this order complete?",
                 "Confirmation",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question
             );

            if (result == DialogResult.Yes) {
                string qry = @"Update tbl_rmsmain set status ='Complete' where MainId =@ID";
                Hashtable ht = new Hashtable();
                ht.Add("ID", id);

                if (RMSClass.SQl(qry, ht) > 0)
                {
                 MessageBox.Show("Saved Successfully");
                }
                GetOrders();
            }
        }
        private void frmRMSKitchview_Load(object sender, EventArgs e)
        {
            GetOrders();
        }
    }
}
