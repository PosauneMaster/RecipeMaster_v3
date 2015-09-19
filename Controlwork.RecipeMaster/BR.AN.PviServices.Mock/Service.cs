using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
    public class Service : Base
    {
        public Service(string name);
        public Service(string name, ServiceCollection services);

        public bool AddMembersToVariableCollection { get; set; }
        public ArrayList ClientNames { get; }
        public CpuCollection Cpus { get; }
        public override string FullName { get; }
        public bool IsStatic { get; set; }
        public string Language { get; set; }
        public LicenceInfo LicenceInfo { get; }
        public ArrayList LoggerCollections { get; }
        public LoggerEntryCollection LoggerEntries { get; }
        public LogicalCollection LogicalObjects { get; }
        public LogicalObjectsUsage LogicalObjectsUsage { get; set; }
        public int MessageLimitation { get; set; }
        public virtual int Port { get; set; }
        public int ProcessTimeout { get; set; }
        public int PVIAutoStart { get; set; }
        public int RetryTime { get; set; }
        public virtual string Server { get; set; }
        public virtual int Timeout { get; set; }
        public VariableCollection Variables { get; }
        public bool WaitForParentConnection { get; set; }

        protected internal override bool Callback(int wParam, int lParam, IntPtr pData, int dataLen, ref ResponseInfo info);
        public override void Connect();
        public virtual void Connect(string server, int port);
        public override void Disconnect();
        public extern static int GetActiveWindow();
        public extern static int GetParent(int owner);
        public extern static int GetWindowContextHelpId(int h);
        public extern static void GetWindowText(int h, StringBuilder s, int nMaxCount);
        public virtual int LoadConfiguration(StreamReader stream);
        public virtual int LoadConfiguration(string fileName);
        protected virtual int LoadConfiguration(StreamReader stream, ConfigurationFlags flags);
        protected virtual int LoadConfiguration(string fileName, ConfigurationFlags flags);
        protected internal int LoadConfiguration(XmlTextReader reader, ConfigurationFlags flags);
        protected override void OnConnected(PviEventArgs e);
        protected internal override void OnError(PviEventArgs e);
        public int RefreshPviClientsList();
        public override void Remove();
        public void RemoveArchive(string path);
        public virtual int SaveConfiguration(string fileName);
        protected virtual int SaveConfiguration(string fileName, ConfigurationFlags flags);
    }
}
