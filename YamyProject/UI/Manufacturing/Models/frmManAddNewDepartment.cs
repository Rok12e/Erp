using DocumentFormat.OpenXml.Spreadsheet;
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
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Models
{
    public partial class frmManAddNewDepartment : Form
    {
        public int MainId;
        public frmManAddNewDepartment()
        {
            InitializeComponent();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtname.Text))
            {
                MessageBox.Show("Can't Saved Department Empty ");
                return;
            }
            string qry1 = ""; //main table
            if (MainId == 0)

            {
                qry1 = @"INSERT INTO  tbl_departments (name,Department) VALUES (@name,@Department); 
                          ";
            }
            else //update
            {
                qry1 = @"update  tbl_departments set name= @name,Department= @Department where id =@ID";
            }
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@ID", MainId);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@Department", "Manufacture");

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            if (MainId == 0) { MainId = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            Utilities.LogAudit(frmLogin.userId, (MainId==0 ? "Add Department" : "Update Department"), "Department", MainId, (MainId==0 ? "Added Department: " : "Updated Department: ") + txtname.Text);
            
            MessageBox.Show("save Successfully...");
            MainId = 0;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
