using System;
using System.Collections.Generic;
using System.Text;
using BR.AN.PviServices;
using System.Reflection;
using System.IO;

namespace ControlWorks.RecipeMaster
{
    public sealed class PviService : IDisposable
    {
        private readonly object m_EventLock = new Object();

        public event EventHandler<PviEventArgs> ServiceConnected;
        public event EventHandler<PviEventArgs> ServiceDisconnected;
        public event EventHandler<PviEventArgs> ServiceError;

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
            Log.LogInfo("Connecting PVI Service");

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
                Log.LogInfo("Mock Service is true. Connecting Mock");
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
                LogPviEvent.LogError("Pvi Service Error", e);

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
                EventHandler<PviEventArgs> temp = this.ServiceError;
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
                LogPviEvent.LogInfo("Pvi Service Disconnected", e);

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
                EventHandler<PviEventArgs> temp = ServiceDisconnected;
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
                LogPviEvent.LogInfo("Pvi Service Connected", e);

                OnServiceConnected(sender, e);
            }
        }

        private void OnServiceConnected(object sender, PviEventArgs e)
        {
            Service service = sender as Service;
            if (service != null)
            {
                EventHandler<PviEventArgs> temp = ServiceConnected;
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
