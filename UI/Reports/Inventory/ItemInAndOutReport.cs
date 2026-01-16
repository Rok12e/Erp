using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class ItemInAndOutReport : Form
    {
        int id;
        string type = "";
        public ItemInAndOutReport(int _id=0,string _type="")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            id = _id;
            type = _type;
            headerUC1.FormText = this.Text;
        }

        private void ItemInAndOutReport_Load(object sender, EventArgs e)
        {
            BindData();
        }

        public void BindData()
        {
            // Optionally show a loading message or disable the grid
            dgvCustomer.Enabled = false;

            // Capture filter values to avoid cross-thread issues
            bool isDateChecked = chkDate.Checked;
            string selectionMethod = cmbSelectionMethod.Text;
            int localId = id;
            DateTime dateFrom = dtFrom.Value.Date;
            DateTime dateTo = dtTo.Value.Date;

            Task.Run(() =>
            {
                DataTable dt;
                string query = @"SELECT ROW_NUMBER() OVER (ORDER BY it.date) AS `SN`,
                        it.date Date,it.type Type,
                        concat('000', it.reference) as `Ref Id`,
                        i.name Name,i.code Code,it.cost_price CostPrice,it.sales_price `Selling Price`,it.qty_in `Qty IN`,it.qty_out `Qty OUT`
                        from tbl_item_transaction it,tbl_items i where it.item_id=i.id and i.state = 0 ";

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                if (!isDateChecked)
                    query += " and it.date >= @dateFrom and it.date <= @dateTo";
                if (selectionMethod == "Default")
                {
                    if (localId > 0)
                    {
                        query += " and i.id=@id";
                        parameters.Add(DBClass.CreateParameter("id", localId.ToString()));
                    }
                }
                else if (selectionMethod == "Opening Qty")
                {
                    query += " and it.type='Opening Qty' ";
                    if (localId > 0)
                    {
                        query += " and i.id=@id";
                        parameters.Add(DBClass.CreateParameter("id", localId.ToString()));
                    }
                }
                else if (selectionMethod == "Purchase")
                {
                    query += " and it.type='Purchase Invoice' ";
                    if (localId > 0)
                    {
                        query += " and i.id=@id";
                        parameters.Add(DBClass.CreateParameter("id", localId.ToString()));
                    }
                }
                else if (selectionMethod == "Sales")
                {
                    query += " and it.type='Sales Invoice' ";
                    if (localId > 0)
                    {
                        query += " and i.id=@id";
                        parameters.Add(DBClass.CreateParameter("id", localId.ToString()));
                    }
                }
                parameters.Add(DBClass.CreateParameter("dateFrom", dateFrom));
                parameters.Add(DBClass.CreateParameter("dateTo", dateTo));
                dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

                // Update UI on the main thread
                this.Invoke((MethodInvoker)delegate
                {
                    dgvCustomer.DataSource = dt;
                    dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                    BeautifyDataGridView();
                    dgvCustomer.Enabled = true;
                });
            });
        }

        public void BindDataOld()
        {
            DataTable dt;
            string query = "";
            //if (cmbSelectionMethod.Text == "Default")
                query = @"SELECT ROW_NUMBER() OVER (ORDER BY it.date) AS `SN`,
                        it.date Date,it.type Type,
                        concat('000', it.reference) as `Ref Id`,
                        i.name Name,i.code Code,it.cost_price CostPrice,it.sales_price `Selling Price`,it.qty_in `Qty IN`,it.qty_out `Qty OUT`
                        
                        from tbl_item_transaction it,tbl_items i where it.item_id=i.id and i.state = 0 ";
            //else
            //    query = @"SELECT ROW_NUMBER() OVER (ORDER BY tbl_item_stock_settlement.date) AS `SN`, 
            //            tbl_item_stock_settlement.date AS Date, 
            //            tbl_item_stock_settlement.id,
            //            tbl_item_stock_settlement.code AS 'INV NO', 
            //            concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.On_Hand As Qty,ts.price 
            //            As 'Cost Price' , ts.new_on_hand  ,ts.minusamount ,ts.plusamount
            //            FROM tbl_item_stock_settlement
            //            INNER JOIN tbl_item_stock_settlement_details ts ON tbl_item_stock_settlement.id = ts.settle_id
            //            inner join tbl_items ti on ts.item_id =ti.id
            //            WHERE tbl_item_stock_settlement.state = 0";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (!chkDate.Checked)
                query += " and it.date >= @dateFrom and it.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
            {
                if (id > 0)
                {
                    query += " and i.id=@id";
                    parameters.Add(DBClass.CreateParameter("id", id.ToString()));
                }
            }
            else if (cmbSelectionMethod.Text == "Opening Qty")
            {
                query += " and it.type='Opening Qty' ";
                if (id > 0)
                {
                    query += " and i.id=@id";
                    parameters.Add(DBClass.CreateParameter("id", id.ToString()));
                }
            }
            else if (cmbSelectionMethod.Text == "Purchase")
            {
                query += " and it.type='Purchase Invoice' ";
                if (id > 0)
                {
                    query += " and i.id=@id";
                    parameters.Add(DBClass.CreateParameter("id", id.ToString()));
                }
            }
            else if (cmbSelectionMethod.Text == "Sales")
            {
                query += " and it.type='Sales Invoice' ";
                if (id > 0)
                {
                    query += " and i.id=@id";
                    parameters.Add(DBClass.CreateParameter("id", id.ToString()));
                }
            }
            //    query += " GROUP BY tbl_item_stock_settlement.id, tbl_item_stock_settlement.date, tbl_item_stock_settlement.code; ";

            //else
            //    query += " GROUP BY tbl_item_stock_settlement.id, tbl_item_stock_settlement.date, tbl_item_stock_settlement.code,ti.code,ti.name,ts.on_hand,ts.price,ts.new_on_hand  ,ts.minusamount ,ts.plusamount; ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = dt;
            dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgvCustomer.Columns["id"].Visible = false;
            //if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

            BeautifyDataGridView();
        }

        private void BeautifyDataGridView()
        {
            // Set the column headers style
            dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateBlue;
            dgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set alternating row colors for better readability
            dgvCustomer.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Set default row color and highlight selection
            dgvCustomer.RowsDefaultCellStyle.BackColor = Color.White;
            dgvCustomer.RowsDefaultCellStyle.SelectionBackColor = Color.SkyBlue;
            dgvCustomer.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Set a border for the rows and columns
            dgvCustomer.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Set the default alignment of the content
            dgvCustomer.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Adjust specific columns for custom formatting (example for currency, date, etc.)
            dgvCustomer.Columns["Qty IN"].DefaultCellStyle.Alignment = dgvCustomer.Columns["Qty OUT"].DefaultCellStyle.Alignment= dgvCustomer.Columns["CostPrice"].DefaultCellStyle.Alignment = dgvCustomer.Columns["Selling Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // For currency format
            //dgvCustomer.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd"; // For date format

            // Adjust column width
            dgvCustomer.Columns["SN"].Width = 60; // Set specific column width
            dgvCustomer.Columns["Ref Id"].Width = 100;
            dgvCustomer.Columns["Qty IN"].Width = 80;
            dgvCustomer.Columns["Qty OUT"].Width = 80;

            // Set the row height for better readability
            dgvCustomer.RowTemplate.Height = 35;

            // Enable sorting on specific columns (optional)
            dgvCustomer.Columns["SN"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvCustomer.Columns["Date"].SortMode = DataGridViewColumnSortMode.Automatic;

            // Optional: Add tooltips to columns
            dgvCustomer.Columns["SN"].ToolTipText = "Serial Number";
            dgvCustomer.Columns["Ref Id"].ToolTipText = "Reference ID";
            dgvCustomer.Columns["Date"].ToolTipText = "Transaction Date";

            // Enable multi-line text for certain columns if needed
            dgvCustomer.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        //private void bindInvoiceItems()
        //{
        //    DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.on_hand as Qty ,ts.price as 'Cost Price' , ts.new_on_hand  from 
        //                                             tbl_item_stock_settlement_details ts inner join tbl_items ti on ts.item_id = ti.id where settle_id = @id",
        //                                             DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
        //    dgvItems.DataSource = dt;
        //    dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //}

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewItem( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            if (dgvCustomer.Rows.Count == 0)
                return;
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            //DBClass.ExecuteNonQuery("UPDATE tbl_items SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            //BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //    //bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            BindData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
