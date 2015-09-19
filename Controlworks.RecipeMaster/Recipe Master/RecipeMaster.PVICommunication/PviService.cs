using System;
using System.Collections.Generic;
using System.Text;
using BR.AN.PviServices;
using System.Reflection;
using System.IO;

namespace BendSheets.PVICommunication
{
    public sealed class PviService : IDisposable
    {
        private readonly object m_EventLock = new Object();
        private EventHandler<PviEventArgs> m_ServiceConnected;
        private EventHandler<PviEventArgs> m_ServiceDisconnected;
        private EventHandler<PviEventArgs> m_ServiceError;

        public event EventHandler<PviEventArgs> ServiceConnected
        {
            add
            {
                lock (m_EventLock) { m_ServiceConnected += value; }
            }
            remove
            {
                lock (m_EventLock) { m_ServiceConnected -= value; }
            }
        }

        public event EventHandler<PviEventArgs> ServiceDisconnected
        {
            add
            {
                lock (m_EventLock) { m_ServiceDisconnected += value; }
            }
            remove
            {
                lock (m_EventLock) { m_ServiceDisconnected -= value; }
            }
        }

        public event EventHandler<PviEventArgs> ServiceError
        {
            add
            {
                lock (m_EventLock) { m_ServiceError += value; }
            }
            remove
            {
                lock (m_EventLock) { m_ServiceError -= value; }
            }
        }

        private PviEventArgs m_PVIEventArgs;

        public PviEventArgs PVIEventArgs
        {
            get { return m_PVIEventArgs; }
            set { m_PVIEventArgs = value; }
        }

        #region Singleton Factory

        private static object _threadLock = new object();
        private static PviService pviService = new PviService();
        private Service m_Service;

        public Service Service
        {
            get { return m_Service; }
        }
        
        public static PviService PviServiceInstance
        {
            get
            {
                if (pviService == null)
                {
                    lock (_threadLock)
                    {
                        pviService = new PviService();
                    }
                }
                return pviService;
            }
        }

        private PviService() { }

        #endregion

        public bool IsConnected
        {
            get
            {
                if (m_Service == null)
                {
                    return false;
                }
                return m_Service.IsConnected;
            }
        }

        public void ConnectPVIService()
        {
            string assembleyPath = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(assembleyPath);

            m_Service = new Service("Service");
            m_Service.Connected += new PviEventHandler(m_Service_Connected);
            m_Service.Disconnected += new PviEventHandler(m_Service_Disconnected);
            m_Service.Error += new PviEventHandler(m_Service_Error);
            m_Service.Connect();
        }

        public void ConnectPVIService(bool mock)
        {
            if (mock)
            {
                m_Service = new Service("Test Service");
                OnServiceConnected(m_Service, new PviEventArgs(String.Empty, String.Empty, 0, String.Empty, BR.AN.PviServices.Action.ErrorEvent));
                return;
            }
            ConnectPVIService();
        }

        private void m_Service_Error(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                if (e.ErrorCode != 0)
                {
                    OnServiceError(sender, e);
                }
            }
        }

        private void OnServiceError(object sender, PviEventArgs e)
        {
            Service service = sender as Service;
            if (service != null)
            {
                UnregisterEvents(service);
                service.Disconnect();
                EventHandler<PviEventArgs> temp = this.m_ServiceError;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private void m_Service_Disconnected(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnServiceDisconnected(sender, e);
            }
        }

        private void OnServiceDisconnected(object sender, PviEventArgs e)
        {
            Service service = sender as Service;
            if (service != null)
            {
                service.Dispose();
                service = null;
                EventHandler<PviEventArgs> temp = this.m_ServiceDisconnected;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private void m_Service_Connected(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnServiceConnected(sender, e);
            }
        }

        private void OnServiceConnected(object sender, PviEventArgs e)
        {
            Service service = sender as Service;
            if (service != null)
            {
                EventHandler<PviEventArgs> temp = this.m_ServiceConnected;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private void UnregisterEvents(Service service)
        {
            service.Connected -= m_Service_Connected;
            service.Error -= m_Service_Error;
            service.Disconnected -= m_Service_Disconnected;
        }

        public void DisconnectPVIService()
        {
            m_Service.Disconnect();
        }

        public void DisconnectPVIService(bool mock)
        {
            if (mock)
            {
                OnServiceDisconnected(this, new PviEventArgs(String.Empty, String.Empty, 0, String.Empty, BR.AN.PviServices.Action.Cancel));
            }
            else
            {
                DisconnectPVIService();
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (m_Service != null)
                    {
                        this.UnregisterEvents(this.m_Service);
                        m_Service.Disconnect();
                        m_Service.Dispose();
                    }
                }
            }
            disposed = true;
        }
    }
}
