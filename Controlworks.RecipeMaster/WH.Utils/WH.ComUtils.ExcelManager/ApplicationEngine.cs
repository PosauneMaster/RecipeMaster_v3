using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;

using Microsoft.Win32;

namespace WH.ComUtils.ExcelManager
{
    [System.Diagnostics.DebuggerNonUserCode()]
    public sealed class ApplicationEngine
    {
        private ApplicationEngine() { }

        public static bool ApplicationInstalled(string keyName)
        {
            return GetRegistryKey(keyName) == null ? false : true;
        }

        public static RegistryKey GetRegistryKey(string keyName)
        {
            RegistryKey regClasses = Registry.ClassesRoot;
            return regClasses.OpenSubKey(keyName);
        }

        public static object CreateApplication(string keyName)
        {
            return Activator.CreateInstance(GetApplicationType(keyName));
        }

        public static Type GetApplicationType(string keyName)
        {
            return Type.GetTypeFromProgID(keyName);
        }
    }
}
