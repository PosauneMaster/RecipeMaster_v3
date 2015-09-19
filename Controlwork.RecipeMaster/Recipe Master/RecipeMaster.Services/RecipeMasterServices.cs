using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Collections.Specialized;
using System.Globalization;


using System.Security.Permissions;
using System.Configuration;
using WH.Utils.Logging;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace RecipeMaster.Services
{
    public sealed class RecipeMasterServices
    {
        private static RecipeMasterServices _services = new RecipeMasterServices();
        private static object _threadLock = new object();

        private RecipeMasterServices()
        {
        }
        public static RecipeMasterServices BendSheetsServicesInstance()
        {
            lock (_threadLock)
            {
                if (_services == null)
                {
                    _services = new RecipeMasterServices();
                }
                return _services;
            }
        }

        public static string LocateProductionFile(string part, string directory)
        {
            string partNumber = MakePartNumber(part);

            if (String.IsNullOrEmpty(partNumber))
            {
                return String.Empty;
            }

            string[] files = Directory.GetFiles(directory, "*.xls", SearchOption.TopDirectoryOnly);

            string[] matchFiles = Array.FindAll(files, delegate(string s) { return s.Contains(partNumber); });

            HybridDictionary parts = partNumbers(matchFiles);

            if(parts.Contains(partNumber))
            {
                return parts[partNumber].ToString();
            }

            return String.Empty;
        }

        private static string MakePartNumber(string partNumber)
        {
            return partNumber.Replace(" ", "");
        }

        private static HybridDictionary partNumbers(string[] matchFiles)
        {
            HybridDictionary parts = new HybridDictionary(matchFiles.Length);

            foreach (string s in matchFiles)
            {
                string[] splitString = s.Split(new char[1] { '\\' });
                string p = splitString[splitString.Length - 1];
                parts.Add(p.Substring(0, p.IndexOf(" ")), s);
            }
            return parts;
        }

        public static string BuildFileName(string partNumber, string directory)
        {
            StringBuilder sb = new StringBuilder(directory);
            if (!sb.ToString().EndsWith(@"\"))
            {
                sb.Append(@"\");
            }
            sb.Append(MakePartNumber(partNumber));
            sb.Append(" BS Production.xls");

            return sb.ToString();
        }

        /// <summary>
        /// Moves the generic Excel template file to the save location
        /// so that data can be written to it.  If there is an existing
        /// file it is saved with a .keep extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string WriteNewProductionFile(string fileName)
        {
            try
            {
                lock (_threadLock)
                {
                    string templateFile = ConfigurationManager.AppSettings["ProductionTemplateExcel"].ToString();
                    string sourceFile = Path.Combine(Directory.GetCurrentDirectory() + @"\Templates", templateFile) + ".xlsx"; ;
                    File.Copy(sourceFile, fileName, true);
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
            return fileName;
        }

        public static void RenameExistingFile(string fileName, bool keepOriginal)
        {
            if (String.IsNullOrEmpty(fileName)) return;
            if (!File.Exists(fileName)) return;

            int keep = 1;
            int index = fileName.IndexOf(".");
            string path = fileName;

            while (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder(fileName);
                sb.Insert(index,String.Concat("(",keep.ToString("N0", Format),")"));
                sb.Append(".keep");

                path = sb.ToString();
                keep++;
            }

            FileInfo fi = new FileInfo(fileName);
            fi.IsReadOnly = false;
            if (keepOriginal)
            {
                fi.CopyTo(path);
            }
            else
            {
                fi.MoveTo(path);
            }
        }

        public static string TemplateDirectory()
        {
            return Path.Combine(Application.StartupPath, "Templates\\");
        }

        public static NumberFormatInfo Format
        {
            get { return new CultureInfo("en-US", false).NumberFormat; }
        }
    }
}
