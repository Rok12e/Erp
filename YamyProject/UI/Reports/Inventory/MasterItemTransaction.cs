using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterItemTransaction : Form
    {
        public MasterItemTransaction()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void MasterItemTransaction_Load(object sender, EventArgs e)
        {
            DataTable dt = DBClass.ExecuteDataTable("select id,name from tbl_items where state = 0 and active = 0");
            cmbSelectionMethod.DataSource = dt;
            cmbSelectionMethod.DisplayMember = "name";
            cmbSelectionMethod.ValueMember = "id";

            dgvCustomer.Columns.Clear();
            dgvCustomer.Columns.Add("SN", "SN");
            dgvCustomer.Columns.Add("Date", "Date");
            dgvCustomer.Columns.Add("Type", "Type");
            dgvCustomer.Columns.Add("RefId", "Ref Id");
            dgvCustomer.Columns.Add("Name", "Name");
            dgvCustomer.Columns.Add("Code", "Code");
            dgvCustomer.Columns.Add("ItemId", "Item Id");
            dgvCustomer.Columns.Add("CostPrice", "Cost Price");
            dgvCustomer.Columns.Add("SellingPrice", "Selling Price");
            dgvCustomer.Columns.Add("QtyIN", "Qty IN");
            dgvCustomer.Columns.Add("QtyOut", "Qty OUT");
            dgvCustomer.Columns.Add("Description", "Description");

            BindInvoices();
        }

        DataTable data;

        public void BindInvoices()
        {
            bool isLoading = false;
            string query = "";
            //query = @"SELECT ROW_NUMBER() OVER (ORDER BY it.date) AS `SN`,
            //            it.date Date,it.type Type,
            //            concat('000', it.reference) as `Ref Id`,
            //            i.name Name,i.code Code,it.item_Id,i.cost_price CostPrice,i.sales_price `Selling Price`,it.qty_in `Qty IN`,it.qty_out,it.description

            //            from tbl_item_transaction it,tbl_items i where it.item_id=i.id and i.state = 0 ";

            //List<MySqlParameter> parameters = new List<MySqlParameter>();
            //if (!chkAllItems.Checked)
            //{
            //    query += " and i.id = @id ";
            //}
            //if (!chkDate.Checked)
            //{


            //    query += " and it.date >= @dateFrom and it.date <= @dateTo";
            //    query += " GROUP BY it.date,it.type,it.reference,it.description,it.qty_in,it.qty_out,i.code,i.name,i.barcode,i.cost_price,i.sales_price,i.id";
            //}
            //parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            //parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            //parameters.Add(DBClass.CreateParameter("id", cmbSelectionMethod.SelectedValue.ToString()));

            query = @"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY it.date) AS SN,
                                DATE_FORMAT(it.date, '%d/%m/%Y') AS `Date`,
                                it.type AS Type,
                                CONCAT('000', it.reference) AS `Ref Id`,
                                i.name AS Name,
                                i.code AS Code,
                                it.item_id,
                                i.cost_price AS CostPrice,
                                i.sales_price AS `Selling Price`,
                                it.qty_in AS `Qty IN`,
                                it.qty_out,
                                it.description
                            FROM 
                                tbl_item_transaction it
                            JOIN 
                                tbl_items i ON it.item_id = i.id
                            WHERE 
                                i.state = 0
                                AND (@id IS NULL OR i.id = @id)
                                AND (@dateFilter = 0 OR (it.date BETWEEN @dateFrom AND @dateTo))
                            ORDER BY 
                                it.date;
                        ";

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("id", chkAllItems.Checked ? DBNull.Value : (object)cmbSelectionMethod.SelectedValue),
                DBClass.CreateParameter("dateFrom", dtFrom.Value.Date),
                DBClass.CreateParameter("dateTo", dtTo.Value.Date),
                DBClass.CreateParameter("dateFilter", chkDate.Checked ? 0 : 1)
            };

            if (isLoading) return;
            isLoading = true;

            Task.Run(() =>
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("SN", typeof(int));
                dt.Columns.Add("Date", typeof(DateTime));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Ref Id", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("item_id", typeof(int));
                dt.Columns.Add("CostPrice", typeof(decimal));
                dt.Columns.Add("Selling Price", typeof(decimal));
                dt.Columns.Add("Qty IN", typeof(decimal));
                dt.Columns.Add("qty_out", typeof(decimal));
                dt.Columns.Add("description", typeof(string));
                this.Invoke(new Action(() =>
                {
                    data = DBClass.ExecuteDataTable(query, parameters.ToArray());
                }));
                //using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                //{
                int rowNumber = 0;
                foreach (DataRow reader in data.Rows)
                {
                    var row = new object[]
                    {
                            ++rowNumber,
                            reader["Date"],
                            reader["Type"],
                            reader["Ref Id"],
                            reader["Name"],
                            reader["Code"],
                            reader["item_id"],
                            reader["CostPrice"],
                            reader["Selling Price"],
                            reader["Qty IN"],
                            reader["qty_out"],
                            reader["description"]
                    };

                    this.Invoke(new Action(() =>
                    {
                        dgvCustomer.Rows.Add(row);
                    }));
                }

                this.Invoke(new Action(() =>
                {
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                    isLoading = false;
                }));
            });

        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewItem(int.Parse(dgvCustomer.SelectedRows[0].Cells["itemId"].Value.ToString())));
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
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindInvoices();
        }

        private void chkAllItems_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllItems.Checked)
            {
                cmbSelectionMethod.Enabled = false;
                loadFilterData("");
            }
            else
            {
                cmbSelectionMethod.Enabled = true;
                loadFilterData(cmbSelectionMethod.SelectedValue.ToString());
            }
        }

        private void cmbSelectionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbSelectionMethod.Enabled)
                return;

            if (!cmbSelectionMethod.Focused || data == null || data.Rows.Count == 0)
                return;

            string selectedId = cmbSelectionMethod.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(selectedId))
                return;

            loadFilterData(selectedId);
        }

        private void loadFilterData(string selectedId)
        {
            if (dgvCustomer == null) return;

            dgvCustomer.Invoke(new MethodInvoker(delegate
            {
                dgvCustomer.Rows.Clear();
            }));

            Task.Run(() =>
            {
                int rowNumber = 0;
                DataView dv = string.IsNullOrEmpty(selectedId)
                    ? new DataView(data)
                    : new DataView(data) { RowFilter = $"item_id = {selectedId}" };

                foreach (DataRowView drv in dv)
                {
                    var row = new object[]
                    {
                        ++rowNumber,
                        drv["Date"],
                        drv["Type"],
                        drv["Ref Id"],
                        drv["Name"],
                        drv["Code"],
                        drv["item_id"],
                        drv["CostPrice"],
                        drv["Selling Price"],
                        drv["Qty IN"],
                        drv["qty_out"],
                        drv["description"]
                    };

                    if (dgvCustomer.InvokeRequired)
                    {
                        dgvCustomer.Invoke(new MethodInvoker(delegate
                        {
                            dgvCustomer.Rows.Add(row);
                        }));
                    }
                    else
                    {
                        dgvCustomer.Rows.Add(row);
                    }
                }
            });
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
