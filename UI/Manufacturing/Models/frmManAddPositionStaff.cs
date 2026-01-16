using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.VisualBasic;
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
using System.Windows.Forms;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Models
{
    public partial class frmManAddPositionStaff : Form
    {
        public int id=0;
        public int IdPostion = 0;

        public frmManAddPositionStaff()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddNewDepartment());
            BindCombos.LoadDepartManufacture(CBDepartment);

        }

        private void frmManAddPositionStaff_Load(object sender, EventArgs e)
        {
           
            BindCombos.LoadDepartManufacture(CBDepartment);
            if (CBDepartment.Text != "")
            {
                btnaddNewDepartment.Visible = false;
                btnUpdateDepartment.Visible = true;
                btnDeleteDepartment.Visible = true;

            }
            else if (CBDepartment.Text == "")
            {
                btnaddNewDepartment.Visible = true;
                btnUpdateDepartment.Visible = false;
                btnDeleteDepartment.Visible = false;

            }

            GetDataPosition();

        }

        private void CBDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataPosition();
            if (CBDepartment.Text != "")
            {
                btnaddNewDepartment.Visible = false;
                btnUpdateDepartment.Visible = true;
                btnDeleteDepartment.Visible = true;

            }

            else if (CBDepartment.Text == "")
            {
                btnaddNewDepartment.Visible = true;
                btnUpdateDepartment.Visible = false;
                btnDeleteDepartment.Visible = false;

            }
        }

        private void CBDepartment_TextChanged(object sender, EventArgs e)
        {
            if (CBDepartment.Text != "")
            {
                btnaddNewDepartment.Visible = false;
                btnUpdateDepartment.Visible = true;
                btnDeleteDepartment.Visible = true;

            }
            else if (CBDepartment.Text == "")
            {
                btnaddNewDepartment.Visible = true;
                btnUpdateDepartment.Visible = false;
                btnDeleteDepartment.Visible = false;


            }
        }

        private void btnUpdateDepartment_Click(object sender, EventArgs e)
        {
            frmManAddNewDepartment frm = new frmManAddNewDepartment();
            frm.MainId =Convert.ToInt16( CBDepartment.SelectedValue);
            frm.txtname.Text = Convert.ToString(CBDepartment.Text);
            RMSClass.blurbackground3(frm);
            BindCombos.LoadDepartManufacture(CBDepartment);
        }

        private void btnDeleteDepartment_Click(object sender, EventArgs e)
        {
            id =Convert.ToInt32( CBDepartment.SelectedValue);
            DialogResult result = MessageBox.Show("Are you Sure want to Delete This Department", "Confirmation",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (dgvItems.Rows.Count > 0 && !dgvItems.Rows[0].IsNewRow)
                {
                    // DataGridView has data
                    MessageBox.Show("Can't Delete This Department Cause Have Positions");
                    return;


                }
                else
                {
                    // DataGridView is empty
                    string qry1 = "Delete from tbl_departments where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId,"Delete Department", "Department", id, "Deleted Department: " + CBDepartment.Text);
                    MessageBox.Show("Delete Successfully");
                    CBDepartment.ResetText();
                    BindCombos.LoadDepartManufacture(CBDepartment);



                }
            }         
        }


        public void GetDataPosition()
        {
            string qry = @" SELECT id,name FROM  tbl_position 
                         WHERE
                         department_id = " + CBDepartment.SelectedValue + " ";
            ListBox lb = new ListBox();
            lb.Items.Add(DgvidPosiotion);
            lb.Items.Add(Position);
            RMSClass.loadData(qry, dgvItems, lb);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtnamePostion.Text))
            {
                MessageBox.Show("Can't Saved Position  Empty ");
                return;
            }
            string qry1 = ""; //main table
            if (IdPostion == 0)

            {
                qry1 = @"INSERT INTO  tbl_position (name,department_id) VALUES (@name,@department_id); 
                          ";
            }
            else //update
            {
                qry1 = @"update  tbl_position set name= @name,department_id= @department_id where id =@ID";
            }
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@ID", IdPostion);
            cmd.Parameters.AddWithValue("@name", txtnamePostion.Text);
            cmd.Parameters.AddWithValue("@department_id", CBDepartment.SelectedValue);

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            if (IdPostion == 0) { IdPostion = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            Utilities.LogAudit(frmLogin.userId, (IdPostion == 0?"Add Position":"Update Position"), "Position", IdPostion, (IdPostion == 0?"Added Position: ":"Updated Position: ") + txtnamePostion.Text);

            MessageBox.Show("save Successfully...");
            IdPostion = 0;
            GetDataPosition();
            txtnamePostion.ResetText();
        }

        private void dgvItems_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1).ToString();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                IdPostion = Convert.ToInt32(dgvItems.CurrentRow.Cells["DgvidPosiotion"].Value);
                txtnamePostion.Text = Convert.ToString(dgvItems.CurrentRow.Cells["Position"].Value);
            }
            if (dgvItems.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                DialogResult result = MessageBox.Show("Are you Sure want to Delete This Batch","Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvItems.CurrentRow.Cells["DgvidPosiotion"].Value);
                    string qry1 = "Delete from  tbl_position where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId,"Delete Position", "Position", id, "Deleted Position: " + txtnamePostion.Text);
                    MessageBox.Show("Delete Successfully");
                    IdPostion = 0;
                    GetDataPosition();
                    txtnamePostion.ResetText();
                }
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
