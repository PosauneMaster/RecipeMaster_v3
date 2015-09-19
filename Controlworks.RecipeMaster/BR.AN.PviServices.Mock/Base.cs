using System;

namespace BR.AN.PviServices
{
    public delegate void PviEventHandler(object sender, PviEventArgs e);

    public abstract class Base : IDisposable
    {
        protected ConnectionType propConnectionType;
        protected bool propHasLinkObject;

        public Base();

        public string Address { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public int ErrorCode { get; }
        public string ErrorText { get; }
        public abstract string FullName { get; }
        public bool HasError { get; }
        public virtual bool HasLinkObject { get; }
        public virtual bool IsConnected { get; }
        public string LinkName { get; set; }
        public string Name { get; }
        public virtual Base Parent { get; }
        public virtual Service Service { get; }
        public object UserData { get; set; }

        public event PviEventHandler Connected;
        public event PviEventHandler Disconnected;
        public event PviEventHandler Error;
        public event PviEventHandler PropertyChanged;

        protected virtual void Call_Connected(PviEventArgs e);
        protected internal virtual bool Callback(int wParam, int lParam, IntPtr pData, int dataLen, ref ResponseInfo info);
        public virtual void Connect();
        public virtual void Disconnect();
        public void Dispose();
        protected bool Fire_Connected(PviEventArgs e);
        protected virtual void Fire_Connected(object sender, PviEventArgs e);
        protected virtual string getLinkDescription();
        protected virtual void OnConnected(PviEventArgs e);
        protected virtual void OnDisconnected(PviEventArgs e);
        protected internal virtual void OnError(PviEventArgs e);
        protected internal virtual void OnError(object sender, PviEventArgs e);
        protected virtual void OnPropertyChanged(PviEventArgs e);
        public virtual void Remove();
    }

    public struct ResponseInfo
    {
        public int Error;
        public int LinkId;
        public int Mode;
        public int Status;
        public int Type;

        public ResponseInfo(int linkId, int mode, int type, int error, int status);
    }

    public class PviEventArgs : EventArgs
    {
        public PviEventArgs(string name, string address, int errorCode, string language, Action action);

        public Action Action { get; }
        public string Address { get; }
        public int ErrorCode { get; }
        public string ErrorText { get; }
        public string Name { get; }
    }

}



