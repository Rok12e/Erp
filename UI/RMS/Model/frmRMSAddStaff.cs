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
using YamyProject.RMS.Class;
using YamyProject.RMS.View;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSAddStaff : frmRMSAddSample
    {
        public frmRMSAddStaff()
        {
            InitializeComponent();
        }
        public int id = 0;
        int code;
        public override void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string GenerateNextEmployeeCode()
        {
          
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 30001;
            }
            return code.ToString("D5");
        }
        public override void btnSave_Click(object sender, EventArgs e)
        {

            GenerateNextEmployeeCode();
            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO `tbl_employee`(code,name,phone,sRole) VALUES (@code,@name,@phone,@srole);";
            }
            else
            {
                qry1 = "Update  tbl_employee set name = @name,phone = @phone,sRole = @srole where id=@id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@code", code);
            ht.Add("@Name", txtname.Text);
            ht.Add("@phone", txtphone.Text);
            ht.Add("@srole", cbrole.Text);

            if (RMSClass.SQl(qry1, ht) > 0)
            {
                Utilities.LogAudit(frmLogin.userId, (id==0 ?"Add Employee": "Update Employee"), "Employee", id, (id ==0? "Added Employee: ":"Updated Employee") + txtname.Text);
                MessageBox.Show("save Successfully...");
                id = 0;
                txtphone.Text = "";
                txtcode.Text = "";
                cbrole.SelectedIndex = -1;
                txtname.Text = "";
                txtname.Focus();
            }
            txtcode.Text = GenerateNextEmployeeCode();

        }

        private void frmRMSAddStaff_Load(object sender, EventArgs e)
        {
            Cbdepartment.Visible = false;
            txtcode.Text = GenerateNextEmployeeCode();
            BindCombos.resturantdepartment(Cbdepartment);
        }

        private void Cbdepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCombos.resturentcombox(cbrole, Cbdepartment.SelectedValue == null ? 0 : (int)Cbdepartment.SelectedValue);
        }

        private void cbrole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
