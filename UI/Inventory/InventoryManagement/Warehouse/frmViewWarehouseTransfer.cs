using CrystalDecisions.CrystalReports.Engine;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Reports.Design;

namespace YamyProject
{
    public partial class frmViewWarehouseTransfer : Form
    {
        DataTable dt = new DataTable();
        int id;
        int itemId;
        int warehouseId;

        public frmViewWarehouseTransfer(int _id=0, int _itemId = 0,int _warehouseId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = "Warehouse - Transfer History";
            this.id = _id;
            this.itemId = _itemId;
            this.warehouseId = _warehouseId;
        }
        public DataTable COMPANYINFO(int id)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company ", DBClass.CreateParameter("@1", id));
        }
        public void ShowReport()
        {
            try
            {
                //CPVITEMTRANS cr = new CPVITEMTRANS();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "ItemTransfer.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath); 
                DataTable companyData = COMPANYINFO(1);
                //DataTable customerdata = Customerdetils(invId.ToString());
                //DataTable salesdata = salesdetils(invId.ToString());
                if (companyData != null)
                {
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["Details"].SetDataSource(dt);
                }
                else
                {
                    MessageBox.Show("No transfer available for the report.");
                    return;
                }

                MasterReportView reportForm = new MasterReportView();
                reportForm.crReportViewer.ReportSource = cr;
                reportForm.crReportViewer.RefreshReport();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void frmViewWarehouseTransfer_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvFrom.DataSource = dt;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvFrom);
        }

        private void LoadData()
        {
            string query = @"select wt.Date, concat(w1.code,' - ',w1.name) as 'Warehouse From',concat(w2.code,' - ',w2.name) as 'Warehouse To',
                                                    concat(i.code,' - ',i.name) as 'Item Name' ,wt.qty, wt.description from tbl_item_warehouse_transaction wt inner join
                                                    tbl_warehouse w1 on wt.warehouse_from = w1.id inner join tbl_warehouse w2 on wt.warehouse_to = w2.id inner join tbl_items i
                                                    on wt.item_id = i.id";

            var parameters = new List<MySqlParameter>();

            if (id > 0)
            {
                query += " and wt.id = @id";
                parameters.Add(new MySqlParameter("id", id));
            }
            if (itemId > 0)
            {
                query += " and i.id = @itemId";
                parameters.Add(new MySqlParameter("itemId", itemId));
            }
            //if (warehouseId > 0)
            //{
            //    query += " and wt.warehouse_to = @warehouseId";
            //    parameters.Add(new MySqlParameter("warehouseId", warehouseId));
            //}

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            if (dt != null && dt.Rows.Count > 0)
                {
                this.dt = dt;
            }
            else
            {
                MessageBox.Show("No transfer available for the report.");
                this.dt = new DataTable();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            ShowReport();
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
    }
}
