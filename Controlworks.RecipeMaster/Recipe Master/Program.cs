using System;
using System.Windows.Forms;

namespace ControlWorks.RecipeMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Log.LogInfo("RecipeMaster Start");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (StartUpForm startForm = new StartUpForm())
            {
                startForm.ShowDialog();
            }

            try
            {
                SingleApplication.Run(new frmMachines());
            }
            catch (Exception ex)
            {
                Log.LogError("Main", ex);
                Application.Exit();
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.LogError("Application_ThreadException", e.Exception);
            MessageBox.Show("A fatal error has occurred and the application must shut down", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.LogError("CurrentDomain_UnhandledException");
            Log.LogError(e.ExceptionObject.ToString());
            MessageBox.Show("A fatal error has occurred and the application must shut down", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}