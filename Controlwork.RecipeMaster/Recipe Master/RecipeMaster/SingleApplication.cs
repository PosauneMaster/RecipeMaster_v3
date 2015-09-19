using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace BendSheets
{
    public sealed class SingleApplication
    {
        #region Private Variables

        private const int SW_RESTORE = 9;
        private static Mutex _mutex;

        #endregion

        #region Constructor
        private SingleApplication() { }
        #endregion

        public static bool IsRunning()
        {
            string assembleyPath = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(assembleyPath);
            
            bool createdNew;

            _mutex = new Mutex(true, String.Concat("Global\\", fileInfo.Name), out createdNew);

            if (createdNew)
            {
                GC.KeepAlive(_mutex);
            }
            return !createdNew;
        }

        public static bool Run(Form form)
        {
            if (IsRunning())
            {
                SwitchToCurrentInstance();
                return false;
            }
            Application.Run(form);
            return true;
        }

        private static void SwitchToCurrentInstance()
        {
            IntPtr hWnd = GetCurrentInstanceHandle();

            if (hWnd != IntPtr.Zero)
            {
                if (NativeMethods.IsIconic(hWnd) != 0)
                {
                    NativeMethods.ShowWindow(hWnd, SW_RESTORE);
                }
            }
            else
            {
                MessageBox.Show("The Bend Sheet application is already running", "Application Active", MessageBoxButtons.OK);
            }

            NativeMethods.SetForegroundWindow(hWnd);
        }

        private static IntPtr GetCurrentInstanceHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);

            foreach (Process p in processes)
            {
                if (p.Id != process.Id &&
                    p.MainModule.FileName == process.MainModule.FileName &&
                    p.MainWindowHandle != IntPtr.Zero)
                {
                    hWnd = p.MainWindowHandle;
                    break;
                }
            }

            return hWnd;
        }
    }
}
