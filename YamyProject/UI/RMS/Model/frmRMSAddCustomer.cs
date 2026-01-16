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

namespace YamyProject.RMS.Model
{
    public partial class frmRMSAddCustomer : Form
    {
        public frmRMSAddCustomer()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        public string OrderType = "";
        public int driverID = 0;
        public string CustName = "";
    

        public int mainID = 0;

        private void frmRMSAddCustomer_Load(object sender, EventArgs e)
        {

            if ((OrderType == "Take Away"))
            {
                lbDriver.Visible = false;
                cbDriver.Visible = false;

            }

            string qry = "select id,name from tbl_employee where sRole Like 'Driver'";
            RMSClass.CBFILL(qry, cbDriver);
            if (mainID > 0)
            {
                cbDriver.SelectedValue = driverID;
            }
        }

        private void cbDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverID  = Convert.ToInt32(cbDriver.SelectedValue);

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            // Now, you can access the Ordertype property
           
        }
    }
}
