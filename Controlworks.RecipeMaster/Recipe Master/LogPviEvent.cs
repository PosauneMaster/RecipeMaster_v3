using BR.AN.PviServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlWorks.RecipeMaster
{
    public static class LogPviEvent
    {
        public static void LogError(string message, PviEventArgs e)
        {
            Log.LogError(FormatMessage(message, e));
        }
        public static void LogDebug(string message, PviEventArgs e)
        {
            Log.LogDebug(FormatMessage(message, e));
        }

        public static void LogInfo(string message, PviEventArgs e)
        {
            Log.LogInfo(FormatMessage(message, e));
        }

        private static string FormatMessage(string message, PviEventArgs e)
        {
            return String.Format("{0}; Action={1}, Address={2}, Error Code={3}, Error Text={4}, Name={5} ",
                message, e.Action, e.Address, e.ErrorCode, e.ErrorText, e.Name);
        }
    }
}
