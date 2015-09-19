using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WH.Utils.Logging;

namespace BendSheets
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
            Log.Write("Application Startup");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (StartUpForm startForm = new StartUpForm())
            {
                startForm.ShowDialog();
            }

            try
            {
                SingleApplication.Run(new BendSheet());
            }
            catch (Exception ex)
            {
                Log.Write("Error on exit");
                Log.Write(ex.ToString());
                Application.Exit();
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Write(LogLevel.FATAL, e.Exception);
            MessageBox.Show("A fatal error has occurred and the application must shut down", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Write(LogLevel.FATAL, e.ExceptionObject);
            MessageBox.Show("A fatal error has occurred and the application must shut down", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}