using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace ControlWorks.RecipeMaster
{
    [Serializable()]
    public sealed class MachineCollection
    {
        public BindingList<Machine> MachineList
        { get; set; }

        public string MasterFilePath
        { get; set; }

        public string ProductionFilePath
        { get; set; }

        public bool Archive
        { get; set; }

        public MachineCollection()
        {
            MachineList = new BindingList<Machine>();
        }

        public void Save(string path)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(MachineCollection));
                TextWriter w = new StreamWriter(path);
                s.Serialize(w, this);
                w.Dispose();
            }
            catch (System.Exception ex)
            {
                Log.LogError("MachineCollection.Save", ex);
            }
        }

        public static MachineCollection LoadSettings(string path)
        {
            MachineCollection machineCollection;

            if (File.Exists(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(MachineCollection));
                using (TextReader textReader = new StreamReader(path))
                {
                    try
                    {
                        machineCollection = (MachineCollection)deserializer.Deserialize(textReader);
                        machineCollection.Initialize();
                    }
                    catch (System.Exception ex)
                    {
                        machineCollection = new MachineCollection();
                        Log.LogError("", ex);
                    }
                }
            }
            else
            {
                machineCollection = new MachineCollection();
            }
            return machineCollection;
        }

        public void Initialize()
        {
            foreach (Machine machine in MachineList)
            {
                machine.Status = "Disconnected";
                machine.IsConnected = false;
                machine.PropertyChanged += new PropertyChangedEventHandler(OnMachinePropertyChanged);
            }
        }

        public void AddMachine(Machine machine)
        {
            machine.PropertyChanged += new PropertyChangedEventHandler(OnMachinePropertyChanged);
            MachineList.Add(machine);
        }

        public void DeleteMachine(Machine machine)
        {
            machine.PropertyChanged -= new PropertyChangedEventHandler(OnMachinePropertyChanged);
            MachineList.Remove(machine);
        }

        [field: NonSerialized]
        public event EventHandler<PropertyChangedEventArgs> MachinePropertyChanged;

        private void OnMachinePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (MachinePropertyChanged != null)
            {
                MachinePropertyChanged(sender, e);
            }
        }
    }
}
