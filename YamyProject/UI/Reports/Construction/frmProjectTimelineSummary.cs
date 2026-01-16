using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YamyProject.UI.Reports.Construction
{
    public partial class frmProjectTimelineSummary : Form
    {
        public frmProjectTimelineSummary()
        {
            InitializeComponent();
        }

        private void frmProjectTimelineSummary_Load(object sender, EventArgs e)
        {
            string query = string.Empty;
            query = @"SELECT plan.id, IFNULL(ted.name, CONCAT('Task ', plan.id)) task_name,plan.start_date,plan.end_date,plan.progress planning_progress, plan.progress,plan.description, plan.date,plan.start_date,plan.end_date from tbl_project_planning plan
                            LEFT JOIN tbl_tender_names ted ON plan.tender_id = ted.id;";

            DataTable dataTable = DBClass.ExecuteDataTable(query);

            LoadGanttChart(dataTable);
        }

        private void LoadChart(DataTable dt)
        {
            gunaChart1.Datasets.Clear();

            var plannedProgress = new GunaBarDataset { Label = "Planned Progress" };
            plannedProgress.FillColors.Add(Color.FromArgb(0, 123, 255)); // Blue

            var actualProgress = new GunaBarDataset { Label = "Actual Progress" };
            actualProgress.FillColors.Add(Color.FromArgb(40, 167, 69)); // Green

            foreach (DataRow row in dt.Rows)
            {
                string taskName = row["task_name"].ToString();
                double planned = row["planning_progress"] != DBNull.Value ? Convert.ToDouble(row["planning_progress"]) : 0;
                double actual = row["progress"] != DBNull.Value ? Convert.ToDouble(row["progress"]) : 0;

                plannedProgress.DataPoints.Add(taskName, planned);
                actualProgress.DataPoints.Add(taskName, actual);
            }

            //gunaChart1.YAxes.Ticks = new Guna.Charts.WinForms.ChartFontTicks()
            //{
            //    BeginAtZero = true,
            //    StepSize = 10
            //};

            gunaChart1.Datasets.Add(plannedProgress);
            gunaChart1.Datasets.Add(actualProgress);
            gunaChart1.Update();
        }

        private void LoadGanttChart(DataTable dt)
        {
            gunaChart1.Datasets.Clear();

            // Find earliest start date
            DateTime minDate = dt.AsEnumerable()
                                 .Min(r => Convert.ToDateTime(r["start_date"]));

            // Offset dataset (transparent color)
            var offsetDataset = new GunaBarDataset
            {
                Label = "Offset Days"
            };
            offsetDataset.FillColors.Add(Color.FromArgb(0, 0, 0, 0)); // Fully transparent

            // Duration dataset (colored)
            var durationDataset = new GunaBarDataset
            {
                Label = "Task Duration"
            };
            durationDataset.FillColors.Add(Color.FromArgb(0, 123, 255)); // Blue

            foreach (DataRow row in dt.Rows)
            {
                string taskName = row["task_name"].ToString();
                DateTime start = Convert.ToDateTime(row["start_date"]);
                DateTime end = Convert.ToDateTime(row["end_date"]);

                int offsetDays = (int)(start - minDate).TotalDays;
                int durationDays = (int)(end - start).TotalDays + 1;

                offsetDataset.DataPoints.Add(taskName, offsetDays);
                durationDataset.DataPoints.Add(taskName, durationDays);
            }

            // Add datasets in order for stacking
            gunaChart1.Datasets.Add(offsetDataset);
            gunaChart1.Datasets.Add(durationDataset);
            gunaChart1.Update();
        }
    }
}
