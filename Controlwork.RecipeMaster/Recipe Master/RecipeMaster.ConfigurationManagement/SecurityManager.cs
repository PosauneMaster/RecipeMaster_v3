using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Configuration;

namespace BendSheets.ConfigurationManagement
{
    public sealed class SecurityManager
    {
        private SecurityManager() {}

        public static void SaveSecuritySetting(bool required, string password)
        {
            SaveSecuritySetting(required, password, ManagerFilePath());
        }

        public static void SaveSecuritySetting(bool required, string password, string path)
        {
            using (StreamWriter sw = new StreamWriter(OpenSecurityFile(path)))
            {
                sw.Write(BuildPassword(required, password));
                sw.Flush();
            }
        }

        public static string CreateHash(string password)
        {
            byte[] byteData = StringToByteArray(password);

            MD5 passwordHash = new MD5CryptoServiceProvider();
            passwordHash.ComputeHash(byteData);

            return Convert.ToBase64String(passwordHash.Hash);
        }

        public static string ReadSecuritySetting()
        {
            return ReadSecuritySetting(ManagerFilePath());
        }

        public static string ReadSecuritySetting(string path)
        {
            string data = String.Empty;

            using (StreamReader sr = new StreamReader(OpenSecurityFile(path)))
            {
                data = sr.ReadLine();
            }
            return data;
        }

        public static bool ValidatePassword(string password)
        {
            return ValidatePassword(password, ManagerFilePath());
        }

        public static bool ValidatePassword(string password, string path)
        {
            return CreateHash(password).Equals(Password(path));
        }

        public static bool RequirePassword()
        {
            return RequirePassword(ManagerFilePath());
        }

        public static bool RequirePassword(string path)
        {
            if (PasswordFileExists(path))
            {
                return GetDataArray(ReadSecuritySetting(path))[0].Equals(TrueHash());
            }
            else
            {
                return false;
            }
        }

        private static string TrueHash()
        {
            return CreateHash(true.ToString());
        }

        private static string BuildPassword(bool req, string password)
        {
            return String.Concat(CreateHash(req.ToString()), " ", CreateHash(password));
        }

        private static string Password(string path)
        {
            string data = ReadSecuritySetting(path);

            if (GetDataArray(data).Length > 1)
            {
                return GetDataArray(data)[1];
            }
            else
            {
                return String.Empty;
            }
        }

        private static string[] GetDataArray(string data)
        {
            char[] separator = new char[1] { ' ' };

            return data.Split(separator);
        }

        public static bool PasswordFileExists()
        {
            return PasswordFileExists(ManagerFilePath());
        }
        public static bool PasswordFileExists(string path)
        {
            return File.Exists(path);
        }

        public static byte[] StringToByteArray(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static FileStream OpenSecurityFile(string path)
        {
            if (File.Exists(path))
            {
                return new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            }
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        }

        public static string ManagerFilePath()
        {
            return String.Concat(Application.StartupPath, ConfigurationManager.AppSettings["SecurityFileName"]);
        }
    }
}
