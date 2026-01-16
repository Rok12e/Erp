using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
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
using YamyProject.RMS.Model;
using YamyProject.UI.CRM.Models;


namespace YamyProject.UI.CRM.Pages
{
    public partial class frmCRMLeads : Form
    {

        public frmCRMLeads()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            guna2DataGridView1.CellPainting += guna2DataGridView1_CellPainting;
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground2(new frmCRMAddLeads());
            GetData();
        }


        public void GetData()
        {
            string qry = @" SELECT ID,LeadName,Date,custcode,CustName,Stage,Amount,Discription,Assigendto,CreateAt FROM tbl_crmcustomer where LeadName like '%" + guna2TextBox1.Text + "%' ";


            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvName);
            lb.Items.Add(Date);
            lb.Items.Add(dgvcustcode);
            lb.Items.Add(Customer);
            lb.Items.Add(Stage);
            lb.Items.Add(Amount);
            lb.Items.Add(dgvDiscrp);
            lb.Items.Add(AssignedTo);
            lb.Items.Add(CreateAt);
            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void frmCRMLeads_Load(object sender, EventArgs e)
        {
            GetData();
        }


        private void guna2DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var stageCell = guna2DataGridView1.Rows[e.RowIndex].Cells["stage"];
            var interactionColIndex = guna2DataGridView1.Columns["interaction"].Index;

            if (e.ColumnIndex == interactionColIndex)
            {
                string stage = stageCell.Value?.ToString();
                int percentage = 0;

                switch (stage)
                {
                    case "Prospecting":
                        percentage = 20;
                        break;
                    case "Appointment":
                        percentage = 30;
                        break;
                    case "Presentation":
                        percentage = 40;
                        break;
                    case "Bought-In":
                        percentage = 65;
                        break;
                    case "Contract":
                        percentage = 85;
                        break;
                    case "Closed Won":
                    case "Closed Lost":
                        percentage = 100;
                        break;
                }

                // حدد اللون حسب المرحلة
                Color barColor;
                if (stage == "Closed Lost")
                {
                    barColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
                }
                else
                {
                    // تدرج من أخضر داكن إلى أخضر فاتح حسب النسبة
                    int greenValue = Math.Min(255, 100 + (percentage * 155 / 100)); // من 100 إلى 255
                    barColor = Color.FromArgb(0, greenValue, 0);
                }

                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                using (SolidBrush brush = new SolidBrush(barColor))
                {
                    Rectangle barRect = new Rectangle(
                        e.CellBounds.X + 1,
                        e.CellBounds.Y + 1,
                        (int)((percentage / 100.0) * (e.CellBounds.Width - 2)),
                        e.CellBounds.Height - 2
                    );
                    e.Graphics.FillRectangle(brush, barRect);
                }

                // لون النص: أبيض أو أسود حسب الخلفية
                Color textColor = (percentage >= 60 || stage == "Closed Lost") ? Color.White : Color.Black;

                string text = $"{percentage}%";
                TextRenderer.DrawText(
                    e.Graphics,
                    text,
                    e.CellStyle.Font,
                    e.CellBounds,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmCRMAddLeads frm = new frmCRMAddLeads();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                frm.cmbCust.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["Customer"].Value);
                frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.dtdate.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["Date"].Value);
                frm.cmbstage.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["Stage"].Value);
                frm.txtamount.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["Amount"].Value);
                frm.txtnotes.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvDiscrp"].Value);
                frm.cmbAssig.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["AssignedTo"].Value);
                frm.dtcreateat.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["CreateAt"].Value);


                RMSClass.blurbackground2(frm);
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this lead?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    string qry1 = "Delete from tbl_crmcustomer where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    MessageBox.Show("Delete Successfully");
                    GetData();
                }
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            
       
            GetData();

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string qry = @" SELECT ID,LeadName,Date,custcode,CustName,Stage,Amount,Discription,Assigendto,CreateAt FROM tbl_crmcustomer where Date Between '%" + guna2TextBox1.Text + "%' and '%" + guna2TextBox1.Text + "%' ";


            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvName);
            lb.Items.Add(Date);
            lb.Items.Add(dgvcustcode);
            lb.Items.Add(Customer);
            lb.Items.Add(Stage);
            lb.Items.Add(Amount);
            lb.Items.Add(dgvDiscrp);
            lb.Items.Add(AssignedTo);
            lb.Items.Add(CreateAt);
            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
