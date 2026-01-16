using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewProjectDashBoard : Form
    {

        public frmViewProjectDashBoard()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Project DashBoard";
            headerUC1.FormText = this.Text;
        }

        private void frmViewProjectDashBoard_Load(object sender, EventArgs e)
        {
            LoadData();
            BindCombos.PopulateProjects(cmbProjectName);
        }

        private void LoadData()
        {
            string queryProjects = "SELECT a.id, date, project_id, start_date, end_date, status, project_type, estimated_budget,b.name location, fund_account_id, created_by, description, fund_period FROM tbl_project_planning a LEFT JOIN tbl_project_sites b ON b.id = a.site WHERE a.state=0";
            string queryEstimates = "SELECT id,'' description FROM tbl_project_tender where state=0 and estimate_status > 0";
            string queryTenders = "SELECT a.id,b.name tender_name,b.code, amount bid_amount, submission_date,(SELECT CONCAT(ROUND(SUM(progress)/100, 2), '% / ', COUNT(*)) AS progress_summary FROM tbl_project_tender_details WHERE tender_id=a.id) status, DESCRIPTION FROM tbl_project_tender a LEFT JOIN tbl_tender_names b ON a.tender_name_id = b.id WHERE a.state=0";

            if (cmbProjectName.Text != "" && !chkProject.Checked)
            {
                queryProjects += " AND project_id=" + cmbProjectName.SelectedValue.ToString();
                queryEstimates += " AND project_id=" + cmbProjectName.SelectedValue.ToString();
                queryTenders += " AND project_id=" + cmbProjectName.SelectedValue.ToString();
            }
            // Execute queries and load data into DataTables
            DataTable dtProjects = DBClass.ExecuteDataTable(queryProjects);
            DataTable dtEstimates = DBClass.ExecuteDataTable(queryEstimates);
            DataTable dtTenders = DBClass.ExecuteDataTable(queryTenders);

            // Update the UI components with the data
            lblTotalProjects.Text = "Total Projects: " + dtProjects.Rows.Count.ToString();
            lblTotalEstimates.Text = "Total Estimates: " + dtEstimates.Rows.Count.ToString();
            lblTotalTenders.Text = "Total Tenders: " + dtTenders.Rows.Count.ToString();

            dgvProjects.DataSource = dtProjects;
            dgvProjectTenders.DataSource = dtTenders;

            // Example of how you could update a chart for project status
            ShowProjectStatusChart(dtProjects);
            BeautifyProjectTendersGrid();
            BeautifyProjectGrid();
        }

        private void ShowProjectStatusChart(DataTable dtProjects)
        {
            // Assuming project status is one of the columns in the projects table, like "status"
            var projectStatuses = dtProjects.AsEnumerable()
                                            .GroupBy(row => row.Field<string>("status"))
                                            .Select(group => new { Status = group.Key, Count = group.Count() })
                                            .ToList();
            // Clear existing data in the chart
            projectStatusChart.Series.Clear();
            projectStatusChart.Legends.Clear();
            // Ensure chart area is defined
            if (projectStatusChart.ChartAreas.Count == 0)
            {
                ChartArea chartArea = new ChartArea("MainChartArea");
                projectStatusChart.ChartAreas.Add(chartArea); // Add the ChartArea
            }
            var series = new Series("Project Status");
            series.ChartType = SeriesChartType.Pie;

            foreach (var status in projectStatuses)
            {
                series.Points.AddXY(status.Status.Substring(0, 4), status.Count);
                series.Points[series.Points.Count - 1].LegendText = status.Status;
            }

            projectStatusChart.Series.Add(series);
            // Optionally, add a legend to the chart
            Legend legend = new Legend("Status Legend");
            projectStatusChart.Legends.Add(legend);
            projectStatusChart.Invalidate();
        }

        private void BeautifyProjectGrid()
        {
           
            // Set AutoGenerateColumns to false to manually specify which columns to show
            dgvProjects.AutoGenerateColumns = false;

            // Hide unnecessary columns
            foreach (DataGridViewColumn col in dgvProjects.Columns)
            {
                if (col.Name == "id" || col.Name == "project_id" ||
                    col.Name == "fund_account_id" || col.Name == "created_by" || col.Name == "description" ||
                    col.Name == "fund_period")
                {
                    col.Visible = false;  // Hide the specified columns
                }
                else
                {
                    col.Visible = true; // Show the other columns
                }
            }

            // Format columns for better readability
            dgvProjects.Columns["date"].HeaderText = "Project Date";
            dgvProjects.Columns["date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvProjects.Columns["date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjects.Columns["start_date"].HeaderText = "Start Date";
            dgvProjects.Columns["start_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvProjects.Columns["start_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjects.Columns["end_date"].HeaderText = "End Date";
            dgvProjects.Columns["end_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvProjects.Columns["end_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjects.Columns["estimated_budget"].HeaderText = "Estimated Budget";
            //dgvProjects.Columns["estimated_budget"].DefaultCellStyle.Format = "C2"; // Currency format
            dgvProjects.Columns["estimated_budget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvProjects.Columns["status"].HeaderText = "Status";
            dgvProjects.Columns["status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjects.Columns["project_type"].HeaderText = "Project Type";
            dgvProjects.Columns["project_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvProjects.Columns["location"].HeaderText = "Location";

            // Auto Resize Columns
            dgvProjects.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvProjects.AutoGenerateColumns = false;
            dgvProjects.AllowUserToResizeColumns = true;
            dgvProjects.AllowUserToResizeRows = false;
            dgvProjects.RowHeadersVisible = false;
            dgvProjects.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dgvProjects.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProjects.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvProjects.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvProjects.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvProjects.DefaultCellStyle.ForeColor = Color.Black;
            dgvProjects.GridColor = Color.Gray;
            // Set column width for readability
            dgvProjects.Columns["status"].Width = 200;
            dgvProjects.Columns["project_type"].Width = 150;
            dgvProjects.Columns["date"].Width = 100;
            dgvProjects.Columns["start_date"].Width = 100;
            dgvProjects.Columns["end_date"].Width = 100;
            dgvProjects.Columns["estimated_budget"].Width = 120;

            // Alternating row colors for better readability
            dgvProjects.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Enable sorting by column
            dgvProjects.AllowUserToOrderColumns = true;

            // Apply header style (e.g., bold, background color, and center alignment)
            dgvProjects.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProjects.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;  // Set the header background color
            dgvProjects.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Set header text color

            // Set row height for better appearance
            dgvProjects.RowTemplate.Height = 30;

            // Optional: Set grid border style for better UI
            dgvProjects.BorderStyle = BorderStyle.Fixed3D;

            // Subscribe to the CellFormatting event for custom row coloring
            dgvProjects.CellFormatting += DgvProjects_CellFormatting;
        }

        private void DgvProjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column is "Status"
            if (dgvProjects.Columns[e.ColumnIndex].Name == "status")
            {
                string status = e.Value.ToString().Trim();

                // Color rows based on the status value
                switch (status)
                {
                    case "Planning":
                        e.CellStyle.BackColor = Color.LightYellow;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Mobilizing":
                        e.CellStyle.BackColor = Color.LightSalmon;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Under Construction":
                        e.CellStyle.BackColor = Color.LightSteelBlue;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Inspection":
                        e.CellStyle.BackColor = Color.LightPink;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Finalizing":
                        e.CellStyle.BackColor = Color.Lavender;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Completed":
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Delayed":
                        e.CellStyle.BackColor = Color.LightCoral;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Canceled":
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Change Order":
                        e.CellStyle.BackColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Commissioning":
                        e.CellStyle.BackColor = Color.LightSeaGreen;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Final Handover":
                        e.CellStyle.BackColor = Color.LightSkyBlue;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    default:
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }

        private void BeautifyProjectTendersGrid()
        {
            // Alternating row colors for better readability
            dgvProjectTenders.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvProjectTenders.AutoGenerateColumns = false;
            dgvProjectTenders.AllowUserToResizeColumns = true;
            dgvProjectTenders.AllowUserToResizeRows = false;
            dgvProjectTenders.RowHeadersVisible = false;
            dgvProjectTenders.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dgvProjectTenders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProjectTenders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvProjectTenders.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvProjectTenders.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvProjectTenders.DefaultCellStyle.ForeColor = Color.Black;
            dgvProjectTenders.GridColor = Color.Gray;

            // Set AutoGenerateColumns to false to manually specify which columns to show
            dgvProjectTenders.AutoGenerateColumns = false;

            // Hide unnecessary columns (example: hide 'id' and 'description' columns)
            foreach (DataGridViewColumn col in dgvProjectTenders.Columns)
            {
                if (col.Name == "id" || col.Name == "description")
                {
                    col.Visible = false;
                }
                else
                {
                    col.Visible = true;
                }
            }

            // Format columns
            dgvProjectTenders.Columns["tender_name"].HeaderText = "Tender Name";
            dgvProjectTenders.Columns["bid_amount"].HeaderText = "Bid Amount";
            //dgvProjectTenders.Columns["contractor"].HeaderText = "Contractor";
            dgvProjectTenders.Columns["status"].HeaderText = "Status";
            dgvProjectTenders.Columns["submission_date"].HeaderText = "Submission Date";

            // Format for 'Bid Amount' column
            //dgvProjectTenders.Columns["bid_amount"].DefaultCellStyle.Format = "C2"; // Currency format
            dgvProjectTenders.Columns["bid_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Format for 'Submission Date' column
            dgvProjectTenders.Columns["submission_date"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgvProjectTenders.Columns["submission_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjectTenders.Columns["status"].Width = 200;
            dgvProjectTenders.Columns["tender_name"].Width = 250;
            dgvProjectTenders.Columns["DESCRIPTION"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjectTenders.Columns["submission_date"].Width = 100;
            dgvProjectTenders.Columns["bid_amount"].Width = 120;

            // Enable sorting
            dgvProjectTenders.AllowUserToOrderColumns = true;

            // Apply header style (e.g., bold and center alignment)
            dgvProjectTenders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProjectTenders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProjectTenders.CellFormatting += dgvProjectTenders_CellFormatting;
        }

        private void dgvProjectTenders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column is "Status"
            if (dgvProjects.Columns[e.ColumnIndex].Name == "status")
            {
                string status = e.Value.ToString().Trim();

                // Color rows based on the status value
                switch (status)
                {
                    case "Planning":
                        e.CellStyle.BackColor = Color.LightYellow;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Mobilizing":
                        e.CellStyle.BackColor = Color.LightSalmon;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Under Construction":
                        e.CellStyle.BackColor = Color.LightSteelBlue;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Inspection":
                        e.CellStyle.BackColor = Color.LightPink;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Finalizing":
                        e.CellStyle.BackColor = Color.Lavender;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Completed":
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Delayed":
                        e.CellStyle.BackColor = Color.LightCoral;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Canceled":
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Change Order":
                        e.CellStyle.BackColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Commissioning":
                        e.CellStyle.BackColor = Color.LightSeaGreen;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Final Handover":
                        e.CellStyle.BackColor = Color.LightSkyBlue;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    default:
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }

        private void cmbProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkEmployee_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkProject_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
                cmbProjectName.Enabled = false;
            else
                cmbProjectName.Enabled = true;
            LoadData();
        }
    }
}
