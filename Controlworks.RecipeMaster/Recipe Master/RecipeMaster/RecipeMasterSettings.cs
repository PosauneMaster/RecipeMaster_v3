using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using WH.Utils.Logging;

namespace BendSheets
{
    [Serializable()]
    public sealed class RecipeMasterSetting : INotifyPropertyChanged
    {
        private string m_MachineName;
        private int m_Destination;
        private bool m_Active;
        private string m_ActiveText;
        private string m_ProductionFile;
        private string m_MasterFile;

        public string MasterFile
        {
            get { return m_MasterFile; }
            set 
            {
                m_MasterFile = value;
                NotifyPropertyChanged("MasterFile");
            }
        }

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

        public string ProductionFile
        {
            get { return m_ProductionFile; }
            set
            {
                m_ProductionFile = value;
                NotifyPropertyChanged("ProductionFile");
            }
        }

        public RecipeMasterSetting()
        {
            this.Active = false;
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

    [Serializable()]
    public sealed class RecipeSettings
    {
        private BindingList<RecipeMasterSetting> m_RecipeSettingList;
        private BindingList<RecipeMasterSetting> m_ActiveList;
        private string m_MasterFilePath;
        private string m_ProductionFilePath;
        private bool m_Archive;

        public string MasterFilePath
        {
            get { return m_MasterFilePath; }
            set { m_MasterFilePath = value; }
        }

        public string ProductionFilePath
        {
            get { return m_ProductionFilePath; }
            set { m_ProductionFilePath = value; }
        }

        public bool Archive
        {
            get { return m_Archive; }
            set { m_Archive = value; }
        }

        public BindingList<RecipeMasterSetting> RecipeSettingList
        {
            get { return m_RecipeSettingList; }
        }

        public void Add(RecipeMasterSetting setting)
        {
            m_RecipeSettingList.Add(setting);
            if (setting.Active)
            {
                m_ActiveList.Add(setting);
            }
        }

        private RecipeSettings()
        {        
            m_RecipeSettingList = new BindingList<RecipeMasterSetting>();
            m_ActiveList = new BindingList<RecipeMasterSetting>();
        }

        public void Save(string path)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(RecipeSettings));
                TextWriter w = new StreamWriter(path);
                s.Serialize(w, this);
                w.Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        public static RecipeSettings LoadSettings(string path)
        {
            RecipeSettings recipeSettings;

            if (File.Exists(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(RecipeSettings));
                using (TextReader textReader = new StreamReader(path))
                {
                    try
                    {
                        recipeSettings = (RecipeSettings)deserializer.Deserialize(textReader);                        
                    }
                    catch (Exception ex)
                    {
                        recipeSettings = new RecipeSettings();
                        Log.Write(ex);
                    }
                }
            }
            else
            {
                recipeSettings = new RecipeSettings();
            }
            return recipeSettings;
        }
    }
}
