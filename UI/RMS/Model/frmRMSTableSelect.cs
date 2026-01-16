using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.UC;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSTableSelect : Form
    {
        public frmRMSTableSelect()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public string Tablename ;
        private void frmRMSTableSelect_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void frmRMSTableSelect_Load(object sender, EventArgs e)
        {
            string qry = "select * from tbl_rmstables ";
            MySqlCommand cmd  = new MySqlCommand(qry, RMSClass.conn());
            DataTable dt = new DataTable(); 
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text = row["tname"].ToString();
                b.Width = 150;
                b.Height = 50;
                b.FillColor = Color.FromArgb(241, 85, 126);
                b.HoverState.FillColor = Color.FromArgb(50, 55, 89);

                //event for click 
                b.Click += new EventHandler(b_click);
                flowLayoutPanel1.Controls.Add(b);


            }
        }

        private void b_click(object sender, EventArgs e)
        {
            Tablename = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }
       

    }
}
