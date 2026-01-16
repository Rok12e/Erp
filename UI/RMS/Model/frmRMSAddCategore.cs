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
    public partial class frmRMSAddCategore : frmRMSAddSample
    {

        public frmRMSAddCategore()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public int id = 0;
        public override void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtname.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("Please Insert Category Name");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            string qry1 = "";
            string categoryCode = "";

            if (id == 0)
            {
                object result = DBClass.ExecuteScalar("SELECT LPAD(MAX(CAST(code AS UNSIGNED)), 3, '0') FROM tbl_item_category;");
                int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                categoryCode = nextId.ToString().PadLeft(3, '0');

                qry1 = "INSERT INTO `tbl_item_category`(`code`,`name`) VALUES (@code,@Name);";
            }
            else
            {
                qry1 = "Update  tbl_item_category set name = (@Name) where id=@id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@code", categoryCode);
            ht.Add("@Name", txtname.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {
                Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Add Category" : "Update Category"), "Category", id, (id == 0 ? "Added Category: " : "Updated Category: ") + txtname.Text);
                MessageBox.Show("save Successfully...");
                id = 0;
                txtname.Text = "";
                txtname.Focus();
            }
        }
    }
}
