using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YamyProject.UI.Default
{
    public partial class frmSplashScreen : Form
    {
        public int NavigationCode { get; private set; } = 0;

        private Timer progressTimer;
        private int progressValue = 0;
        private bool initDone = false;
        private bool progressDone = false;

        public frmSplashScreen()
        {
            InitializeComponent();
            Task.Run(() => ProceedWithInitialization());
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            progressValue = 0;
            gunaProgressBar.Value = 0;
            guna2CircleProgressBar1.Value = 60;
            lblStatus.Text = "Initializing...";

            progressTimer = new Timer();
            progressTimer.Interval = 50;
            progressTimer.Tick += ProgressTimer_Tick;
            progressTimer.Start();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            progressValue += 2;

            if (progressValue >= 100)
            {
                gunaProgressBar.Value = 100;
                progressDone = true;
                progressTimer.Stop();
                TryClose();
            }
            else
            {
                gunaProgressBar.Value = progressValue;
            }
        }

        private void ProceedWithInitialization()
        {
            SafeSetStatus("Connecting to database...");
            string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);
            string connectionString = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);

            if (!IsValidConnection(connectionString))
            {
                NavigationCode = 1; // Go to frmConfig
            }
            else
            {
                SafeSetStatus("Loading company data...");

                string customerCode = Environment.GetEnvironmentVariable("yamy_company_code", EnvironmentVariableTarget.User);
                DataTable table = DBClass.ExecuteDataTable(
                    "SELECT id, `Code`, `Name`, Descriptions, default_company, database_name FROM yamycompany.tbl_company WHERE customer_code = @customerCode",
                    DBClass.CreateParameter("customerCode", customerCode)
                );

                if (table != null && table.Rows.Count == 1)
                {
                    DBClass.Database = table.Rows[0]["database_name"].ToString();
                    NavigationCode = 3; // Go to frmLogin
                }
                else
                {
                    if (table != null && table.Rows.Count > 1)
                    {
                        DataRow defaultCompanyRow = table.AsEnumerable()
                                     .FirstOrDefault(row =>
                                         row["default_company"] != DBNull.Value &&
                                         Convert.ToInt32(row["default_company"]) == 1);

                        if (defaultCompanyRow != null)
                        {
                            DBClass.Database = defaultCompanyRow["database_name"].ToString();
                            NavigationCode = 3; // Go to frmLogin
                        }
                        else
                        {
                            NavigationCode = 2; // Go to frmCompanyList
                        }
                    }
                    else
                        NavigationCode = 2; // Go to frmCompanyList
                }
            }

            SafeSetStatus("Finishing up...");

            initDone = true;
            TryClose();
        }
        static bool IsValidConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return false;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open(); // attempt connection
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        private void TryClose()
        {
            if (initDone && progressDone)
            {
                if (this.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        this.DialogResult = DialogResult.OK;
                    }));
                }
                else
                {
                    // Delay the close until the form is fully initialized
                    this.Shown += (s, e) =>
                    {
                        this.DialogResult = DialogResult.OK;
                    };
                }
            }
        }

        private void SafeSetStatus(string text)
        {
            if (lblStatus != null && !lblStatus.IsDisposed && lblStatus.IsHandleCreated)
            {
                try
                {
                    lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = text));
                }
                catch (ObjectDisposedException)
                {
                    // Form was disposed — safe to ignore
                }
            }
        }
    }
}
