using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterReportView : Form
    {
        public MasterReportView()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void MasterReportView_Load(object sender, EventArgs e)
        {
            //crReportViewer
        }
    }
}
