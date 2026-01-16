using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace YamyProject.UI.CRM.Models
{
    public partial class frmCRMAddLeads : Form
    {
        private string opnlvl = "";
        private EventHandler customerUpdatedHandler;
        private EventHandler itemUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;
        private string customerName = "";
        private string codecust = "";
        public int id = 0;
        int code;
        public frmCRMAddLeads()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCRMAddLeads_Load(object sender, EventArgs e)
        {
      BindCombos.PopulateCustomers(cmbCust);
            if (cmbstage.Text == "Prospecting")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Appointment")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Presentation")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Bought-In")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Contract")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Closed Won")
            {
                opnlvl = "Won";
            }
            else if (cmbstage.Text == "Closed Lost")
            {
                opnlvl = "Lost";
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCust.SelectedValue == null)
            {
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where id = " + cmbCust.SelectedValue.ToString()))
                if (reader.Read())
                {
                    codecust = reader["code"].ToString();
                    customerName = reader["name"].ToString();
                }
                
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbCust.Text))
            {
                MessageBox.Show("Can't Saved Lead Without Customer ");
                return;
            }
            if (string.IsNullOrEmpty(txtname.Text))
            {
                MessageBox.Show("Can't Saved Lead Without Lead Name ");
                return;    
            }
            if (string.IsNullOrEmpty(cmbstage.Text))
            {
                MessageBox.Show("Can't Saved Lead Without Stage level  ");
                return;
            }

            if (string.IsNullOrEmpty(txtamount.Text))
            {
                txtamount.Text ="0";                      
            }
            code = Convert.ToInt32(codecust);
            //GenerateNextEmployeeCode();
            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO `tbl_crmcustomer`(LeadName,custcode,CustName,openlvl,Stage,Date,Amount,Discription,Assigendto,CreateAt) VALUES (@LeadName,@custcode,@CustName,@openlvl,@Stage,@Date,@Amount,@Discription,@Assigendto,@CreateAt);";

            }
            else
            {
                qry1 = "Update  tbl_crmcustomer set LeadName=@LeadName ,custcode = @custcode,CustName = @CustName,openlvl = @openlvl,Stage = @Stage,Date = @Date,Amount = @Amount,Discription = @Discription,Assigendto = @Assigendto,CreateAt = @CreateAt where ID =@ID";
               
            }
            DateTime formattedDate = Convert.ToDateTime(dtdate.Text);
            DateTime formattedDate2 = Convert.ToDateTime(dtcreateat.Text);
            string displayDate = formattedDate.ToString("dd/MM/yyyy");
            string displayDate2 = formattedDate2.ToString("dd/MM/yyyy");
            Hashtable ht = new Hashtable();
            ht.Add("@ID", id);
            ht.Add("@LeadName", txtname.Text);
            ht.Add("@custcode", code);
            ht.Add("@CustName", customerName);
            ht.Add("@openlvl", opnlvl);
            ht.Add("@Stage", cmbstage.Text);
            ht.Add("@Date", formattedDate);
            ht.Add("@Amount", txtamount.Text);
            ht.Add("@Discription", txtnotes.Text);
            ht.Add("@Assigendto", cmbAssig.Text);
            ht.Add("@CreateAt", formattedDate2);
     
            if (RMSClass.SQl(qry1, ht) > 0)
            {
                if (id == 0)
                {
                    Utilities.LogAudit(frmLogin.userId, "Add Lead", "CRM", 0, "Added Lead: " + txtname.Text);
                }
                else
                {
                    Utilities.LogAudit(frmLogin.userId, "Update Lead", "CRM", id, "Updated Lead: " + txtname.Text);
                }
                MessageBox.Show("save Successfully...");
                id = 0;
                txtname.Text = "";
                cmbstage.SelectedIndex = -1;
                txtamount.Text = "";
                txtnotes.Text = "";
                cmbAssig.SelectedIndex = -1;
                txtname.Focus();
            }
          
        }

        private void cmbstage_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (cmbstage.Text == "Prospecting")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Appointment")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Presentation")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Bought-In")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Contract")
            {
                opnlvl = "Open";
            }
            else if (cmbstage.Text == "Closed Won")
            {
                opnlvl = "Won";
            }
            else if (cmbstage.Text == "Closed Lost")
            {
                opnlvl = "Lost";
            }
        }

        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtamount.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
