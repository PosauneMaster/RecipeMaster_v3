using System;
using System.Runtime.InteropServices;

namespace WH.ComUtils.ExcelManager
{
    public static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static Boolean CloseHandle(IntPtr handle);

        public static bool CloseCOMHandle(IntPtr handle)
        {
            return CloseHandle((IntPtr)handle);
        }
    }
}
