using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewItemMovingReport : Form
    {
        private string type;

        public frmViewItemMovingReport(string _type="")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            type = _type;
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }
        
        private void frmViewItemMovingReport_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            
            //cmbSelectionMethod.Text = type == "Customer Summary"? "" : type == "Item Details" ? "Item Wise" : type == "Item Summary" ? "Item Wise" : type== "Aging Summary" ? "A/R Aging Summary" : "";

            BindInvoices();
        }

        private void BindInvoices()
        {
            dgvItems.Enabled = false;

            bool isDateChecked = true;
            DateTime dateFrom = DateTime.Today;
            DateTime dateTo = DateTime.Today;

            this.Invoke((MethodInvoker)(() =>
            {
                isDateChecked = chkDate.Checked;
                dateFrom = dtFrom.Value.Date;
                dateTo = dtTo.Value.Date;
            }));

            Task.Run(() =>
            {
                DataTable dt;
                string query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY i.id) AS `SN`, 
                            i.code,
                            i.name,
                            i.barcode,
                            i.cost_price AS CostPrice,
                            i.sales_price AS SalesPrice,
                            SUM(CASE WHEN t.type = 'Opening Qty' THEN (t.qty_in) ELSE 0 END) AS OpeningQty,
                            SUM(CASE WHEN t.type = 'Purchase Invoice' THEN (t.qty_in) ELSE 0 END) AS Purchase,
                            SUM(CASE WHEN t.type = 'Sales Invoice' THEN (t.qty_out) ELSE 0 END) AS Sales,
                            SUM(CASE WHEN t.type = 'Purchase Return Invoice' THEN (t.qty_out) ELSE 0 END) AS PurchaseReturn,
                            SUM(CASE WHEN t.type = 'Sales Return Invoice' THEN (t.qty_in) ELSE 0 END) AS SalesReturn,
                            SUM(CASE WHEN t.type = 'Damage' THEN (t.qty_out) ELSE 0 END) AS Damage,
                            SUM(t.qty_in - t.qty_out) AS `BalanceQty`
                        FROM
                            tbl_items i
                        INNER JOIN 
                            tbl_item_transaction t ON i.id = t.item_id
                        WHERE 
                            i.type = '11 - Inventory Part' AND i.active = 0
                        ";

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                if (!isDateChecked)
                {
                    query += " AND t.date >= @dateFrom AND t.date <= @dateTo ";
                    parameters.Add(DBClass.CreateParameter("dateFrom", dateFrom));
                    parameters.Add(DBClass.CreateParameter("dateTo", dateTo));
                }

                query += " GROUP BY i.id, i.code, i.name, i.barcode, i.cost_price, i.sales_price";

                dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

                this.Invoke((MethodInvoker)delegate
                {
                    dgvItems.Columns.Clear();
                    dgvItems.Rows.Clear();
                    dgvItems.Enabled = false;

                    foreach (DataColumn col in dt.Columns)
                    {
                        dgvItems.Columns.Add(col.ColumnName, col.ColumnName);
                    }

                    string[] numericColumns = {
                        "CostPrice", "SalesPrice", "OpeningQty", "Purchase",
                        "Sales", "PurchaseReturn", "SalesReturn", "Damage", "BalanceQty"
                    };

                    foreach (DataGridViewColumn column in dgvItems.Columns)
                    {
                        if (numericColumns.Contains(column.Name))
                        {
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }

                    Task.Run(() =>
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!this.IsHandleCreated || this.IsDisposed) return;

                            object[] rowData = dr.ItemArray;

                            this.Invoke((MethodInvoker)delegate
                            {
                                dgvItems.Rows.Add(rowData);
                            });
                        }

                        this.Invoke((MethodInvoker)delegate
                        {
                            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                            dgvItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
                            dgvItems.Enabled = true;
                        });
                    });
                });

            });
        }

        //public void BindInvoices()
        //{
        //    DataTable dt;
        //    string query = "";
        //    query = @"SELECT 
        //                    ROW_NUMBER() OVER (ORDER BY i.id) AS `SN`, 
        //                    i.code,
        //                    i.name,
        //                    i.barcode,
        //                    i.cost_price AS CostPrice,
        //                    i.sales_price AS SalesPrice,
        //                    SUM(CASE WHEN t.type = 'Opening Qty' THEN (t.qty_in) ELSE 0 END) AS OpeningQty,
        //                    SUM(CASE WHEN t.type = 'Purchase Invoice' THEN (t.qty_in) ELSE 0 END) AS Purchase,
        //                    SUM(CASE WHEN t.type = 'Sales Invoice' THEN (t.qty_out) ELSE 0 END) AS Sales,
        //                    SUM(CASE WHEN t.type = 'Purchase Return Invoice' THEN (t.qty_out) ELSE 0 END) AS PurchaseReturn,
        //                    SUM(CASE WHEN t.type = 'Sales Return Invoice' THEN (t.qty_in) ELSE 0 END) AS SalesReturn,
        //                    SUM(CASE WHEN t.type = 'Damage' THEN (t.qty_out) ELSE 0 END) AS Damage,
        //                    SUM(t.qty_in-t.qty_out) AS `BalanceQty`
        //                FROM
        //                    tbl_items i
        //                INNER JOIN 
        //                    tbl_item_transaction t ON i.id = t.item_id
        //                WHERE i.type = '11 - Inventory Part' AND i.active = 0
        //            ";
        //    List<MySqlParameter> parameters = new List<MySqlParameter>();

        //    if (!chkDate.Checked)
        //    {
        //        query += " and t.date >= @dateFrom and t.date <= @dateTo";
        //    }
        //    else
        //    {
        //        query += "GROUP BY i.id, i.code, i.name";
        //    }

        //    parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
        //    parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
        //    dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
        //    dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        //    dgvItems.DataSource = dt;
        //    foreach (DataGridViewColumn column in dgvItems.Columns)
        //    {
        //        if (column.Name == "CostPrice" || column.Name == "SalesPrice" ||
        //            column.Name == "OpeningQty" || column.Name == "Purchase" ||
        //            column.Name == "Sales" || column.Name == "PurchaseReturn" ||
        //            column.Name == "SalesReturn" || column.Name == "Damage" ||
        //            column.Name == "BalanceQty")
        //        {
        //            column.DefaultCellStyle.Format = "N2";
        //            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        }
        //    }
        //    dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        //    dgvItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        //    LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        //}

        //private void BindInvoices()
        //{
        //    dgvItems.Enabled = false;

        //    bool isDateChecked = chkDate.Checked;
        //    DateTime dateFrom = dtFrom.Value.Date;
        //    DateTime dateTo = dtTo.Value.Date;

        //    Task.Run(() =>
        //    {
        //        DataTable dt;
        //        string query = @"SELECT 
        //                                ROW_NUMBER() OVER (ORDER BY i.id) AS `SN`, 
        //                                i.code,
        //                                i.name,
        //                                i.barcode,
        //                                i.cost_price AS CostPrice,
        //                                i.sales_price AS SalesPrice,
        //                                SUM(CASE WHEN t.type = 'Opening Qty' THEN (t.qty_in) ELSE 0 END) AS OpeningQty,
        //                                SUM(CASE WHEN t.type = 'Purchase Invoice' THEN (t.qty_in) ELSE 0 END) AS Purchase,
        //                                SUM(CASE WHEN t.type = 'Sales Invoice' THEN (t.qty_out) ELSE 0 END) AS Sales,
        //                                SUM(CASE WHEN t.type = 'Purchase Return Invoice' THEN (t.qty_out) ELSE 0 END) AS PurchaseReturn,
        //                                SUM(CASE WHEN t.type = 'Sales Return Invoice' THEN (t.qty_in) ELSE 0 END) AS SalesReturn,
        //                                SUM(CASE WHEN t.type = 'Damage' THEN (t.qty_out) ELSE 0 END) AS Damage,
        //                                SUM(t.qty_in - t.qty_out) AS `BalanceQty`
        //                            FROM
        //                                tbl_items i
        //                            INNER JOIN 
        //                                tbl_item_transaction t ON i.id = t.item_id
        //                            WHERE 
        //                                i.type = '11 - Inventory Part' AND i.active = 0
        //                        ";

        //        List<MySqlParameter> parameters = new List<MySqlParameter>();
        //        if (!isDateChecked)
        //        {
        //            query += " AND t.date >= @dateFrom AND t.date <= @dateTo ";
        //            parameters.Add(DBClass.CreateParameter("dateFrom", dateFrom));
        //            parameters.Add(DBClass.CreateParameter("dateTo", dateTo));
        //        }

        //        query += " GROUP BY i.id, i.code, i.name, i.barcode, i.cost_price, i.sales_price";

        //        dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

        //        this.Invoke((MethodInvoker)delegate
        //        {
        //            dgvItems.Columns.Clear();
        //            dgvItems.Rows.Clear();
        //            dgvItems.Enabled = false;

        //            foreach (DataColumn col in dt.Columns)
        //            {
        //                dgvItems.Columns.Add(col.ColumnName, col.ColumnName);
        //            }

        //            string[] numericColumns = {
        //                "CostPrice", "SalesPrice", "OpeningQty", "Purchase",
        //                "Sales", "PurchaseReturn", "SalesReturn", "Damage", "BalanceQty"
        //            };

        //            foreach (DataGridViewColumn column in dgvItems.Columns)
        //            {
        //                if (numericColumns.Contains(column.Name))
        //                {
        //                    column.DefaultCellStyle.Format = "N2";
        //                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //                }
        //            }

        //            Task.Run(() =>
        //            {
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    if (!this.IsHandleCreated || this.IsDisposed) return;

        //                    object[] rowData = dr.ItemArray;

        //                    this.Invoke((MethodInvoker)delegate
        //                    {
        //                        dgvItems.Rows.Add(rowData);
        //                    });
        //                }

        //                this.Invoke((MethodInvoker)delegate
        //                {
        //                    dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        //                    dgvItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        //                    LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        //                    dgvItems.Enabled = true;
        //                });
        //            });
        //        });

        //    });
        //}

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvCustomer.Rows.Count == 0)
            //    return;
            //if (dgvCustomer.Columns["JV NO"].Index == e.ColumnIndex)
            //    _mainForm.openChildForm(new MasterTransactionJournal(_mainForm, dgvCustomer.CurrentRow.Cells["JV NO"].Value.ToString(), "Sales"));
            //else
            //{
                //if (cmbSalesType.SelectedIndex == 1)
                //{//Sales Order
                //    _mainForm.openChildForm(new frmAddSalesOrder(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
                //else if (cmbSalesType.SelectedIndex == 2)
                //{//Sales Quotation
                //    _mainForm.openChildForm(new frmAddSalesQuotation(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
                //else
                //{
                    //_mainForm.openChildForm(new frmAddnvoice(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
            //}
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbCustomer.Enabled = false;
            else
                cmbCustomer.Enabled = true;
            BindInvoices();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
                cmbPaymentMethod.Enabled = false;
            else
                cmbPaymentMethod.Enabled = true;
            BindInvoices();
        }
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //    bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Set formatting and alignment for the specific columns when the editing control is shown
            foreach (DataGridViewColumn column in dgvItems.Columns)
            {
                // Only format these columns
                if (column.Name == "CostPrice" || column.Name == "SalesPrice" ||
                    column.Name == "OpeningQty" || column.Name == "Purchase" ||
                    column.Name == "Sales" || column.Name == "PurchaseReturn" ||
                    column.Name == "SalesReturn" || column.Name == "Damage" ||
                    column.Name == "BalanceQty")
                {
                    // Format the decimal columns (for example, for cost and sales prices)
                    column.DefaultCellStyle.Format = "N2";  // Two decimal places format

                    // Align the content to the right
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // You can also apply resizing, but consider doing this in the Form Load or DataBinding event for better performance
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            if(dtFrom.Focused)
                BindInvoices();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtTo.Focused)
                BindInvoices();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
