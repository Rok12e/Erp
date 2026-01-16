using Guna.UI2.WinForms;
using System;
using System.Collections;
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
using YamyProject.RMS.View;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSAddTable : frmRMSAddSample
    {
        public frmRMSAddTable()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public int id = 0;
        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtname.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("Please Insert Table Name");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO tbl_rmstables (`tname`) VALUES (@Name);";
            }
            else
            {
                qry1 = "Update  tbl_rmstables set tname = (@Name) where tid=@id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtname.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {
                Utilities.LogAudit(frmLogin.userId, (id==0?"Save Table":"Edit Table"), "Table", id, (id==0 ?"Saved Table: ":"Edited Table") + txtname.Text);
                MessageBox.Show("save Successfully...");
                id = 0;
                txtname.Text = "";
                txtname.Focus();
            }


        }

    }

}
