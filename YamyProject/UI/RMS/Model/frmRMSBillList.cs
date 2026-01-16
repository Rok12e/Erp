using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.Reports;
using YamyProject.RMS.View;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSBillList : frmRMSAddSample
    {
        public frmRMSBillList()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public int MainID = 0;
        private void frmRMSBillList_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
            {


                string qry = @"Select MainId ,tableName ,waiterName,orderType,status,Total from tbl_rmsmain
                                where status <> 'Pending' ";
                ListBox lb = new ListBox();
                lb.Items.Add(dgvid);
                lb.Items.Add(dgvtable);
                lb.Items.Add(dgvwaiter);
                lb.Items.Add(dgvType);
                lb.Items.Add(dgvStatus);
                lb.Items.Add(dgvTotal);

                RMSClass.loadData(qry, guna2DataGridView1, lb);
            }
            private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            {
            // for serial no
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {

                count++;
                row.Cells[0].Value = count;


            }
        }
        public DataTable COMPANYINFO(int id)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company LIMIT 1", DBClass.CreateParameter("@1", id));
        }

        public DataTable BillHeaderDETAILS(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT * from tbl_rmsmain WHERE Mainid = @Mainid;", DBClass.CreateParameter("@Mainid", a1));
        }

        public DataTable BillDETAILS(string a1)
        {
            return DBClass.ExecuteDataTable(@"Select d.DetailID,d.proID,p.name,p.id,d.qty,d.price,d.amount,m.tableName,m.waiterName,m.orderType,m.Total,m.received,m.changetot from tbl_rmsmain m 
                                 inner join tbl_rmsdetails d on m.MainId = d.MainID
                                 inner join tbl_items p on p.id = d.proID 
                                 where m.MainId = @Mainid;", DBClass.CreateParameter("@Mainid", a1));
        }
        public void ShowReport()
        {
            try
            {
                //rptBill cr = new rptBill();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "ThermalBill.rpt");//"RollBill.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                DataTable companyData = COMPANYINFO(1);
                DataTable BillHeaderDETAIL = BillHeaderDETAILS(MainID.ToString());
                DataTable BillDETAIL = BillDETAILS(MainID.ToString());
                if (companyData != null)
                {
                    //cr.Subreports["Header"].SetDataSource(companyData);
                    //cr.Subreports["HeaderDetails"].SetDataSource(BillHeaderDETAIL);
                    //cr.Subreports["ItemDetails"].SetDataSource(BillDETAIL);

                    companyData.TableName = "tbl_company";
                    BillHeaderDETAIL.TableName = "tbl_rmsmain";
                    BillDETAIL.TableName = "tbl_rmsmain1";

                    DataSet ds = new DataSet();
                    ds.Tables.Add(companyData.Copy());
                    ds.Tables.Add(BillHeaderDETAIL.Copy());
                    ds.Tables.Add(BillDETAIL.Copy());
                    cr.SetDataSource(ds);
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;
                }

                frmReport reportForm = new frmReport();
                reportForm.crystalReportViewer1.ReportSource = cr;
                reportForm.crystalReportViewer1.RefreshReport();
                reportForm.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {    
               MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
            this.Close();

            }

            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvPrint")
            {
                MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);


                ShowReport();
                //frmReport frm = new frmReport();
                //RMSClass.blurbackground(frm);
                //frmReport FRM = new frmReport();
                //rptBill CR = new rptBill();


            }
        }
    } 
}
