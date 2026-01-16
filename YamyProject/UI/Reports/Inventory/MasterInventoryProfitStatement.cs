using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterInventoryProfitStatement : Form
    {
        public MasterInventoryProfitStatement()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void MasterInventoryProfitStatement_Load(object sender, EventArgs e)
        {
            BindItems();
        }
        public void BindItems()
        {
            string query = @"SELECT ts.date AS DATE,ts.invoice_id AS 'Inv No',  CONCAT(tc.code,' - ',tc.name) AS 'Customer Name',
                                                    CONCAT(ti.code,' - ',ti.name) AS 'Item Name',
                                                    tsd.cost_price AS 'Cost Price', 
                                                    tsd.price AS 'Sales Price',
                                                    tsd.qty AS Qty,
                                                    (tsd.cost_price)*tsd.qty AS 'Cost Amount',
                                                    (tsd.price)*tsd.qty AS 'Sales Amount',										 								
                                                    (tsd.price-tsd.cost_price)*tsd.qty AS 'Profit'
                                                    FROM tbl_sales_details tsd
                                                    INNER JOIN tbl_items ti ON tsd.item_id= ti.id
                                                    INNER JOIN tbl_sales ts ON tsd.sales_id=ts.id
                                                    INNER JOIN tbl_customer tc ON ts.customer_id= tc.id";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (!chkDate.Checked)
            {
                query += " and ts.date >= @dateFrom and ts.date <= @dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }
            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["Item name"].Width = 250;
            dgvCustomer.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["customer name"].MinimumWidth = 180;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindItems();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindItems();
        }
    }
}
