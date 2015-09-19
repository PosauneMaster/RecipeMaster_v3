using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace BR.AN.PviServices
{
    public class ServiceCollection : BaseCollection
    {
        public static Hashtable Services;

        public ServiceCollection();

        public LogicalCollection LogicalObjects { get; }
        public LogicalObjectsUsage LogicalObjectsUsage { get; set; }

        public Service this[string name] { get; }

        public void Add(Service service);
        public void Connect();
        public void Disconnect();
        public int LoadConfiguration(StreamReader stream);
        public int LoadConfiguration(string fileName);
        protected int LoadConfiguration(StreamReader stream, ConfigurationFlags flags);
        protected int LoadConfiguration(string fileName, ConfigurationFlags flags);
        public int SaveConfiguration(string fileName);
        protected int SaveConfiguration(string fileName, ConfigurationFlags flags);
    }
}
