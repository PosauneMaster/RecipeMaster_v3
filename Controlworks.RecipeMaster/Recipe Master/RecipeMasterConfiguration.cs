using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ControlWorks.RecipeMaster
{
    public static class RecipeMasterConfiguration
    {
        public static bool MockConnection
        {
            get
            {
                bool isMock;
                if (Boolean.TryParse(ConfigurationManager.AppSettings["mockConnection"], out isMock))
                {
                    return isMock;
                }
                return false;
            }
        }

        public static bool MockCpuConnection
        {
            get
            {
                bool isMock;
                if (Boolean.TryParse(ConfigurationManager.AppSettings["mockCpuConnection"], out isMock))
                {
                    return isMock;
                }
                return false;
            }
        }

        public static string LogFile
        {
            get
            {
                return ConfigurationManager.AppSettings["LogFile"];
            }
        }

        public static string SecurityFileName
        {
            get
            {
                return ConfigurationManager.AppSettings["SecurityFileName"];
            }
        }

        public static string SendVariablePrefix
        {
            get
            {
                return ConfigurationManager.AppSettings["SendVariablePrefix"];
            }
        }

        public static string ReceiveVariablePrefix
        {
            get
            {
                return ConfigurationManager.AppSettings["ReceiveVariablePrefix"];
            }
        }

        public static string LogLevel
        {
            get
            {
                return ConfigurationManager.AppSettings["LogLevel"];
            }
        }

        public static string ProductionTemplateExcel
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductionTemplateExcel"];
            }
        }

        public static string ControlWorksWebsite
        {
            get
            {
                return ConfigurationManager.AppSettings["ControlWorksWebsite"];
            }
        }
    }
}
