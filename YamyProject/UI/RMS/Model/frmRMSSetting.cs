using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.RMS.Class;

namespace YamyProject.UI.RMS.Model
{
    public partial class frmRMSSetting : Form
    {
        public int CID = 0;
        bool isPrinterDataExist = false;
        public frmRMSSetting()
        {
            InitializeComponent();
        }

        private void LoadPrinters()
        {
            // Clear existing items in case method is called again
            cbp1.Items.Clear();
            cbp2.Items.Clear();
            cbp3.Items.Clear();
            // Get installed printers
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cbp1.Items.Add(printer);
                cbp2.Items.Add(printer);
                cbp3.Items.Add(printer);
            }

            // Optionally, set default printer as selected
            var defaultPrinter = new PrinterSettings().PrinterName;
            if (cbp1.Items.Contains(defaultPrinter))
            {
                cbp1.SelectedItem = defaultPrinter;
            }
            else if (cbp2.Items.Contains(defaultPrinter))
            {
                cbp2.SelectedItem = defaultPrinter;
            }
            else if (cbp3.Items.Contains(defaultPrinter))
            {
                cbp3.SelectedItem = defaultPrinter;
            }

            string query = @"select * from tbl_printconfg";
            using(MySqlDataReader reader = DBClass.ExecuteReader(query))
            {
                if (reader.HasRows) {
                    isPrinterDataExist = true;
                    while (reader.Read())
                    {
                        if (reader["id"].ToString() == "1")
                        {
                            if (cbp1.Items.Contains(defaultPrinter))
                                cbp1.SelectedItem = reader["PrintName"].ToString();
                        }
                        else if (reader["id"].ToString() == "2")
                        {
                            if (cbp2.Items.Contains(defaultPrinter))
                                cbp2.SelectedItem = reader["PrintName"].ToString();
                        }
                        else if (reader["id"].ToString() == "3")
                        {
                            if (cbp3.Items.Contains(defaultPrinter))
                                cbp3.SelectedItem = reader["PrintName"].ToString();
                        }
                    }
                } else
                {
                    // Optionally, set default printer as selected
                    defaultPrinter = new PrinterSettings().PrinterName;
                    if (cbp1.Items.Contains(defaultPrinter))
                    {
                        cbp1.SelectedItem = defaultPrinter;
                    }
                    else if (cbp2.Items.Contains(defaultPrinter))
                    {
                        cbp2.SelectedItem = defaultPrinter;
                    }
                    else if (cbp3.Items.Contains(defaultPrinter))
                    {
                        cbp3.SelectedItem = defaultPrinter;
                    }
                }
            }
        }

        private void frmRMSSetting_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void frmRMSSetting_Load(object sender, EventArgs e)
        {
            LoadPrinters();
            //ComboBox pp;
            //BindCombos.printer1(cbp1);

            LoadGeneral();
        }

        private void LoadGeneral()
        {
            if (BindDataTable.tableGeneralSettings.Rows.Count > 0)
            {
                foreach (DataRow row in BindDataTable.tableGeneralSettings.Rows)
                {
                    string itemName = row["name"].ToString();
                    bool isChecked = Convert.ToBoolean(row["status"]);
                    if (itemName == "PRINT KITCHEN KOT" || itemName == "PRINT BILL")
                    {
                        chkListSettings.Items.Add(itemName, isChecked);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isPrinterDataExist)
            {
                updatePrinter1();
                updatePrinter2();
                updatePrinter3();
            }
            else
            {
                DBClass.ExecuteNonQuery("INSERT INTO tbl_printconfg (id, PrintName) VALUES (1, @name1), (2, @name2), (3, @name3)",
                    new MySqlParameter("@name1", cbp1.Text),
                    new MySqlParameter("@name2", cbp2.Text),
                    new MySqlParameter("@name3", cbp3.Text));

                MessageBox.Show("save Successfully...");
            }
        }

        private void updatePrinter1()
        {
            string qry1 = "Update   tbl_printconfg set PrintName = @name where  id=@id ";

            Hashtable ht = new Hashtable();
            ht.Add("@id", 1);
            ht.Add("@name", cbp1.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {
                MessageBox.Show("save Successfully...");

            }
        }

        private void updatePrinter2()
        {
            string qry1 = "Update   tbl_printconfg set PrintName = @name where  id=@id ";

            Hashtable ht = new Hashtable();
            ht.Add("@id", 2);
            ht.Add("@name", cbp2.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {


            }
        }
            private void updatePrinter3()
        {
            string qry1 = "Update   tbl_printconfg set PrintName = @name where  id=@id ";

            Hashtable ht = new Hashtable();
            ht.Add("@id", 3);
            ht.Add("@name", cbp3.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {


            }
        }

    }
}
