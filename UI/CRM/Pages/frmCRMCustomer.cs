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
using YamyProject.UI.CRM.Models;

namespace YamyProject.UI.CRM.Pages
{
    public partial class frmCRMCustomer : Form
    {

        private int idcd = 0;
        public frmCRMCustomer()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground2(new frmViewCustomer());
            GetData();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground2(new frmCRMAddLeads());
        }




        public void GetData()
        {
            //string qry = @"SELECT P.id, P.code, P.name, P.sales_price, C.name 
            //               FROM `tbl_items` P 
            //               INNER JOIN tbl_item_category C ON C.id = P.category_id where p.name like '%" + txtSearch.Text + "%' and p.code > 1300001";


            string qry = @" SELECT
                         c.code AS code,
                         c.name AS CustomerName,
                         c.main_phone AS Phone,
                         c.email AS Email,
                         c.city AS City,
                         c.city AS Country,
                         COUNT(cr.id) AS TotalLeads,
                         SUM(CASE WHEN cr.openlvl = 'Open' THEN 1 ELSE 0 END) AS OpenLeads,
                         SUM(CASE WHEN cr.openlvl = 'Lost' THEN 1 ELSE 0 END) AS LostLeads,
                         SUM(CASE WHEN cr.openlvl = 'Won' THEN 1 ELSE 0 END) AS WonLeads
                         FROM 
                         tbl_customer c
                         LEFT JOIN 
                         tbl_crmcustomer cr ON c.code = cr.custcode
                         WHERE
                         c.name LIKE '%" + guna2TextBox1.Text + "%' GROUP BY c.id, c.name, c.main_phone, c.email, c.city;";

            ListBox lb = new ListBox();
            lb.Items.Add(dgvCode);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvphone);
            lb.Items.Add(dgvemail);
            lb.Items.Add(dgvCity);
            lb.Items.Add(Dgvcontry);
            lb.Items.Add(dgvLeads);
            lb.Items.Add(OpenLeads);
            lb.Items.Add(LostLeads);
            lb.Items.Add(WonLeads);
            RMSClass.loadData(qry, dgvcustomercrm, lb);
        }




        private void frmCRMCustomer_Load(object sender, EventArgs e)
        {
    
            GetData();
           
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
