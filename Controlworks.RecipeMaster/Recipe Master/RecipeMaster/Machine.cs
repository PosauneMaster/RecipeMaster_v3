using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using WH.Utils.Logging;
using System.Collections.ObjectModel;
using BR.AN.PviServices;
using System.Globalization;
using System.Windows.Forms;
using RecipeMaster.Services;
using System.Configuration;
using System.Diagnostics;

namespace BendSheets
{
    [Serializable()]
    public class Machine : INotifyPropertyChanged
    {
        private string m_MachineName;
        private int m_Destination;
        private bool m_IsConnected;
        private string m_MasterFile;
        private string m_ProductionFile;
        private string m_FileName;
        private string m_Status;
        private bool m_Active;
        private string m_ActiveText;
        private RecipeTemplates m_Templates;
        private Cpu m_Cpu;

        public string TemplatePath
        { get; set; }

        public string Template
        { get; set; }

        public string MachineName
        {
            get { return m_MachineName; }
            set 
            {
                m_MachineName = value;
                NotifyPropertyChanged("MachineName");
            }
        }

        public int Destination
        {
            get { return m_Destination; }
            set 
            {
                m_Destination = value;
                NotifyPropertyChanged("Destination");
            }
        }

        public bool IsConnected
        {
            get { return m_IsConnected; }
            set 
            {
                m_IsConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        public string MasterFile
        {
            get { return m_MasterFile; }
            set 
            {
                m_MasterFile = value;
                NotifyPropertyChanged("MasterFile");
            }
        }

        public string ProductionFile
        {
            get { return m_ProductionFile; }
            set 
            {
                m_ProductionFile = value;
                NotifyPropertyChanged("ProductionFile");
            }
        }

        public string MasterFileDirectory
        { get; set; }

        public string ProductionFileDirectory
        { get; set; }

        public string FileName
        {
            get { return m_FileName; }
            set 
            {
                m_FileName = value;
                NotifyPropertyChanged("FileName");
            }
        }
        public string Status
        {
            get 
            {
                if (String.IsNullOrEmpty(m_Status))
                {
                    m_Status = "Disconnected";
                }
                return m_Status; 
            }
            set 
            {
                m_Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public bool Active
        {
            get { return m_Active; }
            set
            {
                m_Active = value;
                ActiveText = m_Active ? "Active" : "Inactive";
                NotifyPropertyChanged("Active");
            }
        }

        public string ActiveText
        {
            get { return m_ActiveText; }
            set
            {
                m_ActiveText = value;
                NotifyPropertyChanged("ActiveText");
            }
        }

        public Machine()
        {
            m_MachineName = String.Empty;
            m_Destination = 0;
            m_IsConnected = false;
            m_MasterFile = String.Empty;
            m_ProductionFile = String.Empty;
            m_FileName = String.Empty;
            m_Status = "Disconnected";
            m_ActiveText = "Inactive";

        }

        public void OpenTemplate()
        {
            m_Templates = new RecipeTemplates(TemplatePath);
        }

        public void OpenTemplate(string path)
        {
            try
            {
                TemplatePath = path;
                Template = Path.GetFileNameWithoutExtension(path);
                OpenTemplate();
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        VariableCollections m_VariableCollections;
        //public void GetExcelData(string fileName, string templateName)
        //{
        //    try
        //    {
        //        RecipeData.MapDataFromExcel(fileName, m_VariableCollections.SendCollection);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        Log.Write(LogLevel.ERROR, ex);
        //    }
        //}

        public void GetExcelData(string fileName)
        {
            try
            {
                RecipeData.MapDataFromExcel(fileName, m_VariableCollections.SendCollection);

            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }


        public ReadOnlyCollection<RecipeVariable> GetVariableCollection()
        {
            return new ReadOnlyCollection<RecipeVariable>(m_VariableCollections.SendCollection);
        }

        private Stack<Variable> m_VariableStack = new Stack<Variable>();

        private Variable InitVariable(string name)
        {
            return InitVariable(name, null);
        }

        private Variable InitVariable(string name, VariableEventHandler method)
        {
            Variable v = null;
            try
            {
                v = new Variable(m_Cpu, name);
                v.Error += new PviEventHandler(v_Error);
                v.Connected += new PviEventHandler(v_Connected);
                v.WriteValueAutomatic = false;
                if (method != null)
                {
                    v.ValueChanged += method;
                }
                m_VariableStack.Push(v);
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
            return v;
        }

        private List<RecipeVariable> m_RecipeVariables = new List<RecipeVariable>();
        public void SetCpu(Cpu cpu)
        {
            try
            {
                this.m_Cpu = cpu;

                InitVariable("updateProductionFile", new VariableEventHandler(filesRequested));
                InitVariable("updateMasterFile", new VariableEventHandler(filesRequested));

                for (int i = 0; i < 150; i++)
                {
                    InitVariable(String.Format("productionFileDirectory[{0}]", i));
                    InitVariable(String.Format("masterFileDirectory[{0}]", i));
                }

                Variable v = new Variable(m_Cpu, "communicationsActive");
                v.Error += new PviEventHandler(v_Error);
                v.Connected += new PviEventHandler(v_Connected);
                v.WriteValueAutomatic = false;
                v.Active = true;
                v.Connect();

                Variable fileName = new Variable(m_Cpu, "fileName");
                fileName.Error += new PviEventHandler(v_Error);
                fileName.Active = true;
                fileName.Connect();
                fileName.ValueChanged += new VariableEventHandler(fileName_ValueChanged);

                Variable getProduction = new Variable(m_Cpu, "btnGetProductionData");
                getProduction.Error += new PviEventHandler(v_Error);
                getProduction.Active = true;
                getProduction.Connect();
                getProduction.ValueChanged += new VariableEventHandler(getProduction_ValueChanged);

                Variable rpFileName = new Variable(m_Cpu, "runningPart.fileName");
                rpFileName.Error += new PviEventHandler(v_Error);
                rpFileName.Active = true;
                rpFileName.Connect();

                OpenTemplate(TemplatePath);

                m_VariableCollections = RecipeData.CreateVariableList(m_Templates.TemplateList);
                CreateReceiveVariables(m_VariableCollections.ReceiveCollection);
                CreateSendVariables(m_VariableCollections.SendCollection);

            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void ReadFileName()
        {
            Variable v2 = m_Cpu.Variables["fileName"];
            v2.ReadValue();
            v2.ValueRead += new PviEventHandler(v2_ValueRead);

        }

        void v2_ValueRead(object sender, PviEventArgs e)
        {
            Variable v = sender as Variable;
        }

        void fileName_ValueChanged(object sender, VariableEventArgs e)
        {
            Variable v = sender as Variable;
        }

        private void getProduction_ValueChanged(object sender, VariableEventArgs e)
        {
            try
            {
                Variable getProd = sender as Variable;
                if (getProd.Value)
                {
                    Variable v2 = m_Cpu.Variables["fileName"];
                    v2.ReadValue(true);
                    string fileName = v2.Value.ToString();

                    string path = Path.Combine(this.ProductionFile, fileName + ".xlsx");
                    this.GetExcelData(path);
                    this.Send();
                    getProd.Value.Assign(0);
                    getProd.WriteValue();
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void filesRequested(object sender, VariableEventArgs e)
        {
            try
            {
                Variable v = sender as Variable;

                if (v == null)
                {
                    return;
                }

                if (!v.Value)
                {
                    return;
                }

                v.Value.Assign(0);
                v.WriteValue();
                Variable var;

                if (v.Name == "updateProductionFile")
                {
                    var = m_Cpu.Variables["productionFileDirectory"];
                    SendFiles(var, new DirectoryInfo(this.m_ProductionFile));
                }
                else if (v.Name == "masterFileDirectory")
                {
                    var = m_Cpu.Variables["masterFileDirectory"];
                    SendFiles(var, new DirectoryInfo(this.m_MasterFile));
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void SendFiles(Variable v, DirectoryInfo dirInfo)
        {
            try
            {
                FileInfo[] files = dirInfo.GetFiles("*.xls", SearchOption.TopDirectoryOnly);

                List<string> list = new List<string>();

                foreach (FileInfo fi in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(fi.Name));
                }
                list.Sort();

                int length = list.Count == 150 ? 150 : list.Count;

                for (int i = 0; i < length; i++)
                {
                    string varName = String.Format("productionFileDirectory[{0}]", i);
                    Variable variable = m_Cpu.Variables[varName];
                    variable.Value.Assign(list[i]);
                    variable.WriteValue();
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
            return;
        }

        void v_Activated(object sender, PviEventArgs e)
        {
        }

        void v_Connected(object sender, PviEventArgs e)
        {
            try
            {
                Variable v = sender as Variable;
                if (v.Name == "communicationsActive")
                {
                    v.Value.Assign(1);
                    v.WriteValue();
                }
                if (this.m_VariableStack.Count > 0)
                {
                    v = m_VariableStack.Pop();
                    v.Active = true;
                    v.Connect();
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        void v_Error(object sender, PviEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                Variable v = sender as Variable;
                if (v != null)
                {
                    sb.AppendFormat("Variable Name: {0}\n", v.Name);
                    sb.AppendFormat("Action: {0}\n", e.Action);
                    sb.AppendFormat("Address: {0}\n", e.Address);
                    sb.AppendFormat("Error Code: {0}\n", e.ErrorCode);
                    sb.AppendFormat("Error Text: {0}\n", e.ErrorText);
                    sb.AppendFormat("Name: {0}\n", e.Name);
                    sb.AppendLine("Exception:");
                    sb.AppendLine(e.ToString());
                }
                Log.Write(sb.ToString());
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void SendInactive()
        {
            try
            {
                Variable v = m_Cpu.Variables["communicationsActive"];
                v.Value.Assign(0);
                v.WriteValue();
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void Send()
        {
            try
            {
                foreach (RecipeVariable rv in this.m_VariableCollections.SendCollection)
                {
                    this.m_Cpu.Variables[rv.Name].Value.Assign(rv.Value);
                    this.m_Cpu.Variables[rv.Name].WriteValue();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void CreateReceiveVariables(IEnumerable<RecipeVariable> receiveCollection)
        {
            try
            {
                Variable variable = new Variable(m_Cpu, "btnSendProductionData");

                variable.ValueChanged += new VariableEventHandler(SendProductionData_ValueChanged);
                variable.Error += new PviEventHandler(v_Error);
                variable.Active = true;
                variable.Connect();
                variable.Value.Assign(0);
                variable.WriteValue();
                
                m_RecipeVariables.Clear();

                foreach (RecipeVariable rv in receiveCollection)
                {
                    variable = new Variable(m_Cpu, rv.Name);
                    variable.Active = true;
                    variable.Connect();
                    m_RecipeVariables.Add(rv);
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        private void CreateSendVariables(IEnumerable<RecipeVariable> variableCollection)
        {
            try
            {
                foreach (RecipeVariable rv in variableCollection)
                {
                    Variable variable = new Variable(m_Cpu, rv.Name);
                    variable.ValueChanged += new VariableEventHandler(variable_ValueChanged);
                    variable.Active = true;
                    variable.Connect();
                    variable.Disconnected += new PviEventHandler(variable_Disconnected);
                    variable.Error += new PviEventHandler(v_Error);

                    Debug.Print(rv.Name);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private readonly string MASTER_FILE = @"masterPress.xlsx";
        void SendProductionData_ValueChanged(object sender, PviEventArgs e)
        {
            try
            {
                Variable rpFileName = m_Cpu.Variables["runningPart.fileName"];
                rpFileName.ReadValue(true);
                string fileName = rpFileName.Value.ToString();

                Variable variable = sender as Variable;
                if (variable.Value)
                {
                    variable.WriteValueAutomatic = false;
                    variable.Value.Assign(false);
                    variable.WriteValue();

                    foreach (RecipeVariable rv in m_RecipeVariables)
                    {
                        rv.Value = m_Cpu.Variables[rv.Name].Value;
                    }

                    string destinationFile = Path.Combine(m_ProductionFile, fileName) + ".xlsx";
                    string sourceFile = Path.Combine(MasterFile, MASTER_FILE);

                    if (File.Exists(destinationFile))
                    {
                        RecipeMasterServices.RenameExistingFile(destinationFile, true);
                    }
                    File.Copy(sourceFile, destinationFile, true);
                    RecipeData.WriteExcelData(destinationFile, m_Templates, m_RecipeVariables);
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        void rpFileName_ValueRead(object sender, PviEventArgs e)
        {
            Variable v = sender as Variable;
        }

        void variable_Error(object sender, PviEventArgs e)
        {
        }

        void variable_Disconnected(object sender, PviEventArgs e)
        {
        }

        void variable_ValueChanged(object sender, VariableEventArgs e)
        {
        }

        #region INotifyPropertyChanged Members

        [field: NonSerialized]   
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }


}
