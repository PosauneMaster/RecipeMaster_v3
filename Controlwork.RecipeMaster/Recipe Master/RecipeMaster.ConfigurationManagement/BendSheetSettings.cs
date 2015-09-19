using System;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using RecipeMaster.Services;

[assembly: CLSCompliant(false)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace BendSheets.ConfigurationManagement
{
    public sealed class BendSheetsConfigurationHandler : ConfigurationSection
    {
        public BendSheetsConfigurationHandler() { }

        public BendSheetsConfigurationHandler(string configs)
        {
            BendSheetConfigs = configs;
        }

        [ConfigurationProperty("bendSheetConfigs", DefaultValue = "BendSheetConfigs", IsRequired = true)]
        public string BendSheetConfigs
        {
            get { return (string)this["bendSheetConfigs"]; }
            set { this["bendSheetConfigs"] = value; }
        }

        [ConfigurationProperty("bendConfig")]
        public BendSheetsConfigurationElementHandler BendConfig
        {
            get { return (BendSheetsConfigurationElementHandler)this["bendConfig"]; }
            set { this["bendConfig"] = value; }
        }
    }

    public sealed class BendSheetsConfigurationElementHandler : ConfigurationElement
    {
        public BendSheetsConfigurationElementHandler(){}

        public BendSheetsConfigurationElementHandler(string archive, string cell)
        {
            Archive = archive;
            Cell = cell;
        }

        [ConfigurationProperty("archive", DefaultValue = "true", IsRequired = true)]
        public string Archive
        {
            get { return (string)this["archive"]; }
            set { this["archive"] = value; }
        }

        [ConfigurationProperty("cell", DefaultValue = "Q3", IsRequired = true)]
        public string Cell
        {
            get { return (string)this["cell"]; }
            set { this["cell"] = value; }
        }

    }

    public sealed class FilePathsHandler : ConfigurationSection
    {
        public FilePathsHandler() { }

        public FilePathsHandler(string filePaths)
        {
            FilePaths = filePaths;
        }

        [ConfigurationProperty("paths", DefaultValue = "Paths", IsRequired = true)]
        public string FilePaths
        {
            get { return (string)this["paths"]; }
            set { this["paths"] = value; }
        }

        [ConfigurationProperty("filePathElement1")]
        public FilePathsElementHandler FilePathElement1
        {
            get { return (FilePathsElementHandler)this["filePathElement1"]; }
            set { this["filePathElement1"] = value; }
        }
        [ConfigurationProperty("filePathElement2")]
        public FilePathsElementHandler FilePathElement2
        {
            get { return (FilePathsElementHandler)this["filePathElement2"]; }
            set { this["filePathElement2"] = value; }
        }
        [ConfigurationProperty("filePathElement3")]
        public FilePathsElementHandler FilePathElement3
        {
            get { return (FilePathsElementHandler)this["filePathElement3"]; }
            set { this["filePathElement3"] = value; }
        }
        [ConfigurationProperty("filePathElement4")]
        public FilePathsElementHandler FilePathElement4
        {
            get { return (FilePathsElementHandler)this["filePathElement4"]; }
            set { this["filePathElement4"] = value; }
        }
        [ConfigurationProperty("filePathElement5")]
        public FilePathsElementHandler FilePathElement5
        {
            get { return (FilePathsElementHandler)this["filePathElement5"]; }
            set { this["filePathElement5"] = value; }
        }
    }

    public sealed class FilePathsElementHandler : ConfigurationElement
    {
        public FilePathsElementHandler() { }

        public FilePathsElementHandler(string master, string production)
        {
            MasterFilePath = master;
            ProductionFilePath = production;
        }

        [ConfigurationProperty("masterFilePath", DefaultValue = @"c:\", IsRequired = true)]
        public string MasterFilePath
        {
            get { return (string)this["masterFilePath"]; }
            set { this["masterFilePath"] = value; }
        }

        [ConfigurationProperty("productionFilePath", DefaultValue = @"c:\", IsRequired = true)]
        public string ProductionFilePath
        {
            get { return (string)this["productionFilePath"]; }
            set { this["productionFilePath"] = value; }
        }
    }

    public sealed class MachineTabHandler : ConfigurationSection
    {
        public MachineTabHandler() { }

        public MachineTabHandler(string tab)
        {
            Tab = tab;
        }

        [ConfigurationProperty("tabs", DefaultValue = "Machines", IsRequired = true)]
        public string Tab
        {
            get { return (string)this["tabs"]; }
            set { this["tabs"] = value; }
        }

        [ConfigurationProperty("machineTabElement1")]
        public MachineTabElementHandler MachineTab1
        {
            get { return (MachineTabElementHandler)this["machineTabElement1"]; }
            set { this["machineTabElement1"] = value; }
        }

        [ConfigurationProperty("machineTabElement2")]
        public MachineTabElementHandler MachineTab2
        {
            get { return (MachineTabElementHandler)this["machineTabElement2"]; }
            set { this["machineTabElement2"] = value; }
        }

        [ConfigurationProperty("machineTabElement3")]
        public MachineTabElementHandler MachineTab3
        {
            get { return (MachineTabElementHandler)this["machineTabElement3"]; }
            set { this["machineTabElement3"] = value; }
        }

        [ConfigurationProperty("machineTabElement4")]
        public MachineTabElementHandler MachineTab4
        {
            get { return (MachineTabElementHandler)this["machineTabElement4"]; }
            set { this["machineTabElement4"] = value; }
        }
        [ConfigurationProperty("machineTabElement5")]
        public MachineTabElementHandler MachineTab5
        {
            get { return (MachineTabElementHandler)this["machineTabElement5"]; }
            set { this["machineTabElement5"] = value; }
        }
    }

    public sealed class MachineTabElementHandler : ConfigurationElement
    {
        public MachineTabElementHandler() { }

        public MachineTabElementHandler(string machineName, int destination)
        {
            MachineName = machineName;
            Destination = destination;
        }

        [ConfigurationProperty("machineName", DefaultValue = "Machine Name", IsRequired = true)]
        public string MachineName
        {
            get { return (string)this["machineName"]; }
            set { this["machineName"] = value; }
        }

        [ConfigurationProperty("destination", DefaultValue = (int)0, IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 99, ExcludeRange = false)]
        public int Destination
        {
            get { return (int)this["destination"]; }
            set { this["destination"] = value; }
        }
    }

    public sealed class BendSheetSettings
    {
        private static Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static readonly string FILE_PATH_SECTION = "filePathsSection";
        private static readonly string TAB_SECTION = "machineTabSection";

        private BendSheetSettings() { }

        public static int getDestination(int i)
        {
            if (i == 0)
            {
                return Destination1;
            }
            if (i == 1)
            {
                return Destination2;
            }
            if (i == 2)
            {
                return Destination3;
            }
            if (i == 3)
            {
                return Destination4;
            }
            if (i == 4)
            {
                return Destination5;
            }
            return 0;
        }

        public static string getProductionDirectory(int index)
        {
            if (index == 0)
            {
                return ProductionFile1;
            }
            if (index == 1)
            {
                return ProductionFile2;
            }
            if (index == 2)
            {
                return ProductionFile3;
            }
            if (index == 3)
            {
                return ProductionFile4;
            }
            if (index == 4)
            {
                return ProductionFile5;
            }
            return String.Empty;
        }

        public static string getMachineName(int index)
        {
            if (index == 0)
            {
                return MachineName1;
            }
            if (index == 1)
            {
                return MachineName2;
            }
            if (index == 2)
            {
                return MachineName3;
            }
            if (index == 3)
            {
                return MachineName4;
            }
            if (index == 4)
            {
                return MachineName5;
            }

            return String.Empty;
        }

        public static string MasterFile1
        {
            get { return FilePathSection.FilePathElement1.MasterFilePath.ToString(); }
            set { SavePathSection(value, ProductionFile1, 1); }
        }

        public static string ProductionFile1
        {
            get { return FilePathSection.FilePathElement1.ProductionFilePath.ToString(); }
            set { SavePathSection(MasterFile1, value, 1); }
        }

        public static string MasterFile2
        {
            get { return FilePathSection.FilePathElement2.MasterFilePath.ToString(); }
            set { SavePathSection(value, ProductionFile2, 2); }
        }

        public static string ProductionFile2
        {
            get { return FilePathSection.FilePathElement2.ProductionFilePath.ToString(); }
            set { SavePathSection(MasterFile2, value, 2); }
        }

        public static string MasterFile3
        {
            get { return FilePathSection.FilePathElement3.MasterFilePath.ToString(); }
            set { SavePathSection(value, ProductionFile3, 3); }
        }

        public static string ProductionFile3
        {
            get { return FilePathSection.FilePathElement3.ProductionFilePath.ToString(); }
            set { SavePathSection(MasterFile3, value, 3); }
        }

        public static string MasterFile4
        {
            get { return FilePathSection.FilePathElement4.MasterFilePath.ToString(); }
            set { SavePathSection(value, ProductionFile4, 4); }
        }

        public static string ProductionFile4
        {
            get { return FilePathSection.FilePathElement4.ProductionFilePath.ToString(); }
            set { SavePathSection(MasterFile4, value, 4); }
        }

        public static string ProductionFile5
        {
            get { return FilePathSection.FilePathElement5.ProductionFilePath.ToString(); }
            set { SavePathSection(MasterFile5, value, 5); }
        }

        public static string MachineName1
        {
            get { return MachineTabSection.MachineTab1.MachineName.ToString(); }
            set { SaveTabSection(value, Destination1, 1); }
        }

        public static int Destination1
        {
            get { return MachineTabSection.MachineTab1.Destination; }
            set { SaveTabSection(MachineName1, value, 1); }
        }

        public static string MachineName2
        {
            get { return MachineTabSection.MachineTab2.MachineName.ToString(); }
            set { SaveTabSection(value, Destination2, 2); }
        }

        public static int Destination2
        {
            get { return MachineTabSection.MachineTab2.Destination; }
            set { SaveTabSection(MachineName2, value, 2); }
        }

        public static string MachineName3
        {
            get { return MachineTabSection.MachineTab3.MachineName.ToString(); }
            set { SaveTabSection(value, Destination3, 3); }
        }

        public static int Destination3
        {
            get { return MachineTabSection.MachineTab3.Destination; }
            set { SaveTabSection(MachineName3, value, 3); }
        }

        public static string MachineName4
        {
            get { return MachineTabSection.MachineTab4.MachineName.ToString(); }
            set { SaveTabSection(value, Destination4, 4); }
        }

        public static int Destination4
        {
            get { return MachineTabSection.MachineTab4.Destination; }
            set { SaveTabSection(MachineName4, value, 4); }
        }

        public static string MasterFile5
        {
            get { return FilePathSection.FilePathElement5.MasterFilePath.ToString(); }
            set { SavePathSection(value, ProductionFile5, 5); }
        }

        
        public static string MachineName5
        {
            get { return MachineTabSection.MachineTab5.MachineName.ToString(); }
            set { SaveTabSection(value, Destination5, 5); }
        }

        public static int Destination5
        {
            get { return MachineTabSection.MachineTab5.Destination; }
            set { SaveTabSection(MachineName5, value, 5); }
        }

        public static ReadOnlyCollection<int> ActiveDestinations
        {
            get { return Destinations(); }
        }

        private static ReadOnlyCollection<int> Destinations()
        {
            List<int> list = new List<int>(4);

            if (Destination1 > 0 && Destination1 < 100)
            {
                list.Add(Destination1);
            }
            if (Destination2 > 0 && Destination2 < 100)
            {
                list.Add(Destination2);
            }
            if (Destination3 > 0 && Destination3 < 100)
            {
                list.Add(Destination3);
            }
            if (Destination4 > 0 && Destination4 < 100)
            {
                list.Add(Destination4);
            }
            if (Destination5 > 0 && Destination5 < 100)
            {
                list.Add(Destination4);
            }
            return new ReadOnlyCollection<int>(list);
        }

        public static bool MockConnection
        {
            get
            {
                bool result;
                if (bool.TryParse(ConfigurationManager.AppSettings["mockConnection"], out result))
                {
                    return result;
                }
                return false;
            }
        }

        public static bool MockCpuConnection
        {
            get
            {
                bool result;
                if (bool.TryParse(ConfigurationManager.AppSettings["mockCpuConnection"], out result))
                {
                    return result;
                }
                return false;
            }
        }

        public static int TotalNumberMachines
        {
            get
            {
                int result;
                if (Int32.TryParse(ConfigurationManager.AppSettings["totalNumberMachines"], out result))
                {
                    return result;
                }
                return 0;
            }
            set 
            {
                ConfigurationManager.AppSettings["totalNumberMachines"] = value.ToString();
            }
        }

        public static string GetProductionTemplate(int index)
        {
            return ProductionTemplates()[index];
        }

        private static List<string> ProductionTemplates()
        {
            List<string> list = new List<string>();
            list.Add(ProductionTemplatePath(ConfigurationManager.AppSettings["ProductionTemplate0"]));
            list.Add(ProductionTemplatePath(ConfigurationManager.AppSettings["ProductionTemplate1"]));
            list.Add(ProductionTemplatePath(ConfigurationManager.AppSettings["ProductionTemplate2"]));
            list.Add(ProductionTemplatePath(ConfigurationManager.AppSettings["ProductionTemplate3"]));
            list.Add(ProductionTemplatePath(ConfigurationManager.AppSettings["ProductionTemplate4"]));

            return list;
        }

        private static string ProductionTemplatePath(string fileName)
        {
            return String.Concat(BendSheetsServices.TemplateDirectory(), fileName);
        }

        private static FilePathsHandler CreateFilePathElement(string master, string production, int index)
        {
            FilePathsHandler handler = new FilePathsHandler(FilePathSection.FilePaths);
            handler.FilePathElement1 = FilePathSection.FilePathElement1;
            handler.FilePathElement2 = FilePathSection.FilePathElement2;
            handler.FilePathElement3 = FilePathSection.FilePathElement3;
            handler.FilePathElement4 = FilePathSection.FilePathElement4;
            handler.FilePathElement5 = FilePathSection.FilePathElement5;

            switch (index)
            {
                case 1:
                    handler.FilePathElement1 = new FilePathsElementHandler(master, production);
                    break;
                case 2:
                    handler.FilePathElement2 = new FilePathsElementHandler(master, production);
                    break;
                case 3:
                    handler.FilePathElement3 = new FilePathsElementHandler(master, production);
                    break;
                case 4:
                    handler.FilePathElement4 = new FilePathsElementHandler(master, production);
                    break;
                case 5:
                    handler.FilePathElement5 = new FilePathsElementHandler(master, production);
                    break;
            }

            return handler;
        }

        private static MachineTabHandler CreateMachineTabElement(string machineName, int destination, int index)
        {
            MachineTabHandler handler = new MachineTabHandler(MachineTabSection.Tab);
            handler.MachineTab1 = MachineTabSection.MachineTab1;
            handler.MachineTab2 = MachineTabSection.MachineTab2;
            handler.MachineTab3 = MachineTabSection.MachineTab3;
            handler.MachineTab4 = MachineTabSection.MachineTab4;
            handler.MachineTab5 = MachineTabSection.MachineTab5;
            switch (index)
            {
                case 1:
                    handler.MachineTab1 = new MachineTabElementHandler(machineName, destination);
                    break;
                case 2:
                    handler.MachineTab2 = new MachineTabElementHandler(machineName, destination);
                    break;
                case 3:
                    handler.MachineTab3 = new MachineTabElementHandler(machineName, destination);
                    break;
                case 4:
                    handler.MachineTab4 = new MachineTabElementHandler(machineName, destination);
                    break;
                case 5:
                    handler.MachineTab5 = new MachineTabElementHandler(machineName, destination);
                    break;
            }

            return handler;
        }

        private static void SaveTabSection(string machineName, int destination, int index)
        {
            MachineTabHandler handler = CreateMachineTabElement(machineName, destination, index);
            SaveTabSection(handler);
        }

        private static void SaveTabSection(MachineTabHandler handler)
        {
            _config.Sections.Remove(TAB_SECTION);
            _config.Sections.Add(TAB_SECTION, handler);
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(TAB_SECTION);
        }

        private static void SavePathSection(string master, string production, int index)
        {
            FilePathsHandler handler = CreateFilePathElement(master, production, index);
            SavePathSection(handler);
        }

        private static void SavePathSection(FilePathsHandler handler)
        {
            _config.Sections.Remove(FILE_PATH_SECTION);
            _config.Sections.Add(FILE_PATH_SECTION, handler);
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(FILE_PATH_SECTION);
        }

        private static FilePathsHandler FilePathSection
        {
            get { return ConfigurationManager.GetSection(FILE_PATH_SECTION) as FilePathsHandler; }
        }

        private static MachineTabHandler MachineTabSection
        {
            get { return ConfigurationManager.GetSection("machineTabSection") as MachineTabHandler; }
        }

        private static BendSheetsConfigurationHandler BendSheetConfigSection
        {
            get { return ConfigurationManager.GetSection("BendSheetConfigSection") as BendSheetsConfigurationHandler; }
        }

        public static bool Archive
        {
            get { return bool.Parse(BendSheetConfigSection.BendConfig.Archive); }
            set { SaveBendSheetConfigSection(value.ToString(), SavePathCell); }
        }

        public static string SavePathCell
        {
            get { return BendSheetConfigSection.BendConfig.Cell.ToString(); }
            set
            { SaveBendSheetConfigSection(Archive.ToString(), value);
            }
        }

        private static void SaveBendSheetConfigSection(string archive, string cell)
        {
            BendSheetsConfigurationHandler handler = new BendSheetsConfigurationHandler(BendSheetConfigSection.BendSheetConfigs);
            handler.BendConfig = new BendSheetsConfigurationElementHandler(archive, cell);

            _config.Sections.Remove("BendSheetConfigSection");
            _config.Sections.Add("BendSheetConfigSection", handler);
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("BendSheetConfigSection");
        }
    }
}
