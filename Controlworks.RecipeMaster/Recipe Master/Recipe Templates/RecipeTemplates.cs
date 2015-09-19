using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BendSheets
{
    public class RecipeTemplates
    {
        private BindingList<RecipeTemplateItem> m_Items;
        private Dictionary<string, RecipeTemplateItem> m_ItemDictionary;

        public RecipeTemplateItem this[string name]
        {
            get { return m_ItemDictionary[name]; }
        }

        public BindingList<RecipeTemplateItem> TemplateList
        {
            get { return m_Items; }
        }

        public int Count
        {
            get { return m_ItemDictionary.Count; }
        }

        public RecipeTemplates()
        {
            m_Items = new BindingList<RecipeTemplateItem>();
            m_ItemDictionary = new Dictionary<string, RecipeTemplateItem>();
        }

        public RecipeTemplates(string path)
        {
            m_Items = new BindingList<RecipeTemplateItem>();
            m_ItemDictionary = new Dictionary<string, RecipeTemplateItem>();
            Load(path);
        }

        public void DeleteTemplate(RecipeTemplateItem item)
        {
            if (m_ItemDictionary.ContainsKey(item.SendName))
            {
                m_Items.Remove(item);
                m_ItemDictionary.Remove(item.SendName);
            }
        }

        public void AddTemplate(RecipeTemplateItem item)
        {
            if (item == null)
            {
                return;
            }

            if (m_ItemDictionary.ContainsKey(item.SendName))
            {
                m_ItemDictionary.Remove(item.SendName);
                m_Items.Remove(item);
            }
            m_ItemDictionary.Add(item.SendName, item);
            m_Items.Add(item);
        }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BindingList<RecipeTemplateItem>));
            using (TextWriter w = new StreamWriter(path))
            {
                serializer.Serialize(w, m_Items);
                w.Close();
            }
        }

        public void Load(string path)
        {
            m_Items.Clear();
            m_ItemDictionary.Clear();

            if (File.Exists(path))
            {
                BindingList<RecipeTemplateItem> deserializedItems;
                XmlSerializer deserializer = new XmlSerializer(typeof(BindingList<RecipeTemplateItem>));
                using (TextReader textReader = new StreamReader(path))
                {
                    deserializedItems = deserializer.Deserialize(textReader) as BindingList<RecipeTemplateItem>;
                }

                if (m_Items != null)
                {
                    foreach (RecipeTemplateItem item in deserializedItems)
                    {
                        AddTemplate(item);
                    }
                }
            }
        }
    }
}
