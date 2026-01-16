using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Default;
using YamyProject.UI.Settings;
using static HardwareInfo;

namespace YamyProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LocalizationManager.ApplySavedLanguage();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int navigate = 0;

            using (frmSplashScreen splash = new frmSplashScreen())
            {
                DialogResult result = splash.ShowDialog();
                navigate = splash.NavigationCode;
            }

            //Trial check
            if (!TrialManager.IsTrialValid() && !ActivationManager.IsActivated())
            {
                // Trial expired and not activated → show activation form
                using (FrmLicense frm = new FrmLicense())
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        // User closed the form or failed activation
                        return; // Exit app
                    }
                }
            }

            // Navigate based on the code returned by the splash screen
            if (navigate == 1)
            {
                Application.Run(new frmConfig());
            }
            else if (navigate == 2)
            {
                Application.Run(new frmCompanyList());
            }
            else if (navigate == 3)
            {
                Application.Run(new frmLogin());
            }
            else
            {
                // fallback, e.g. show config if something went wrong
                Application.Run(new frmConfig());
            }
        }
    }
}
