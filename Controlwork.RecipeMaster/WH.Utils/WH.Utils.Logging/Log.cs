using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Reflection;

using System.Security.Permissions;
using System.Diagnostics;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace WH.Utils.Logging
{
    public sealed class Log
    {
        private static object m_Locker = new object();
        private static LogLevel m_logLevel;

        static Log()
        {
            SetLogLevel();
        }

        private static void SetLogLevel()
        {
            string level;
            try
            {
                level = ConfigurationManager.AppSettings["LogLevel"].ToString();
                m_logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), level, true);
            }
            catch(Exception ex)
            {
                m_logLevel = LogLevel.ERROR;
            }
        }

        public static string LogFileName()
        {
            StringBuilder sb = new StringBuilder();

            lock (m_Locker)
            {
                sb.Append(LoggerDirectory());
                sb.Append(ConfigurationManager.AppSettings["LogFileName"]);
                sb.Append(PreviousMonday().ToString(" MM-dd-yyyy"));
                sb.Append(".log");
            }

            return sb.ToString();
        }

        private static DateTime PreviousMonday()
        {
            DateTime date = System.DateTime.Now;

            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        public static void Write(LogLevel logLevel, string text)
        {
            lock (m_Locker)
            {
                if (!File.Exists(LogFileName()))
                {
                    using (StreamWriter sw = File.CreateText(LogFileName()))
                    {
                        Write(text, sw);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(LogFileName()))
                    {
                        Write(text, sw);
                    }
                }
            }
        }

        private static void Write(string text, TextWriter tw)
        {
            lock (m_Locker)
            {
                WriteHeader(text, tw);
                tw.WriteLine(text);
            }
        }

        private static void WriteHeader(string text, TextWriter tw)
        {
            tw.Write(tw.NewLine);
            tw.WriteLine("*************************************");
            tw.WriteLine("        Message Logged           ");
            tw.WriteLine(String.Concat(DateTime.Now.ToLongDateString(), " ", DateTime.Now.ToLongTimeString()));

            tw.WriteLine("*************************************");
            tw.Write(tw.NewLine);
        }
        
        public static void Write(object objException)
        {
            Write(LogLevel.ERROR, objException);
        }
        public static void Write(LogLevel logLevel, object objException)
        {
            lock (m_Locker)
            {
                Exception ex = objException as Exception;

                if (ex == null)
                {
                    return;
                }

                if (!File.Exists(LogFileName()))
                {
                    using (StreamWriter sw = File.CreateText(LogFileName()))
                    {
                        Write(ex, sw);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(LogFileName()))
                    {
                        Write(ex, sw);
                    }
                }
            }
        }

        private static void Write(Exception ex, TextWriter tw)
        {
            WriteHeader(ex, tw);
            WriteException(ex, tw);
            tw.Close();
        }

        private static void WriteHeader(Exception ex, TextWriter tw)
        {
            tw.Write(tw.NewLine);
            tw.WriteLine("*************************************");
            tw.WriteLine("        Exception Logged           ");
            tw.WriteLine(String.Concat(DateTime.Now.ToLongDateString(), " ", DateTime.Now.ToLongTimeString()));

            tw.WriteLine("*************************************");
            tw.Write(tw.NewLine);
            tw.Write("File Name: ");
            if (ex != null)
            {
                tw.WriteLine(ex.Data["fileName"]);
            }
        }

        private static void WriteException(Exception ex, TextWriter tw)
        {
            tw.Write("Source: ");
            tw.WriteLine(ex.Source);
            tw.Write("Message: ");
            tw.WriteLine(ex.Message);
            tw.WriteLine("Stack Trace:");
            tw.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                tw.Write(tw.NewLine);
                tw.WriteLine("Inner Exception");
                WriteException(ex.InnerException, tw);
            }
            tw.Flush();
        }

        private static string LoggerDirectory()
        {
            string logDirectory;

            try
            {
                logDirectory = ConfigurationManager.AppSettings["LogFileDirectory"].ToString();
                Directory.CreateDirectory(logDirectory);
            }
            catch (Exception ex)
            {
                logDirectory = DefaultLoggerDirectory();
            }

            return logDirectory + "\\";
        }

        private static string DefaultLoggerDirectory()
        {
            return Path.Combine(Application.StartupPath, "Logs");
        }
    }

    public enum LogLevel
    {
        DEBUG = 0,
        ERROR,
        FATAL
    }
}
