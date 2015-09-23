using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ControlWorks.RecipeMaster
{
    internal static class NativeMethods
    {
        #region Win32 API

        [DllImport("user32.dll")]
        internal static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern int IsIconic(IntPtr hWnd);
        #endregion

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static Boolean CloseHandle(IntPtr handle);

        public static bool CloseCOMHandle(IntPtr handle)
        {
            return CloseHandle((IntPtr)handle);
        }
    }
}
