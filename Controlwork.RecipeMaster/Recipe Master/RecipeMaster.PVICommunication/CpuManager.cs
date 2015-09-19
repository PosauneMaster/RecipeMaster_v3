using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Globalization;
using RecipeMaster.Services;
using BR.AN.PviServices;
using BendSheets.ConfigurationManagement;
using System.Configuration;
using WH.Utils.Logging;

namespace BendSheets.PVICommunication
{
    public sealed class CpuManager : IDisposable
    {
        private readonly byte m_SourceStationId = 100;
        private readonly object m_EventLock = new Object();
        private EventHandler<PviEventArgs> m_CpuConnected;
        private EventHandler<PviEventArgs> m_CpuDisconnected;
        private EventHandler<PviEventArgs> m_CpuError;

        public event EventHandler<PviEventArgs> CpuConnected
        {
            add
            {
                lock (m_EventLock) { m_CpuConnected += value; }
            }
            remove
            {
                lock (m_EventLock) { m_CpuConnected -= value; }
            }
        }
        public event EventHandler<PviEventArgs> CpuDisconnected
        {
            add
            {
                lock (m_EventLock) { m_CpuDisconnected += value; }
            }
            remove
            {
                lock (m_EventLock) { m_CpuDisconnected -= value; }
            }
        }

        public event EventHandler<PviEventArgs> CpuError
        {
            add
            {
                lock (m_EventLock) { m_CpuError += value; }
            }
            remove
            {
                lock (m_EventLock) { m_CpuError -= value; }
            }
        }

        public CpuManager()
        {
        }

        public void ConnectCpu(string cpuName, int destinationStation)
        {
            try
            {
                Cpu cpu = null;
                if (PviService.PviServiceInstance.Service.Cpus.ContainsKey(cpuName))
                {
                    cpu = PviService.PviServiceInstance.Service.Cpus[cpuName];
                }
                else
                {
                    cpu = new Cpu(PviService.PviServiceInstance.Service, cpuName);
                }
                cpu.Connection.DeviceType = DeviceType.TcpIp;
                cpu.Connection.TcpIp.SourceStation = this.m_SourceStationId;
                cpu.Connection.TcpIp.DestinationStation = (byte)destinationStation;

                bool mockConnection;

                if (!Boolean.TryParse(ConfigurationManager.AppSettings["mockCpuConnection"], out mockConnection))
                {
                    mockConnection = false;
                }

                if (mockConnection)
                {
                    cpu.Connected += new PviEventHandler(cpu_MockConnected);
                    cpu.Error += new PviEventHandler(cpu_MockError);
                    cpu.Disconnected += new PviEventHandler(cpu_Disconnected);
                }
                else
                {
                    cpu.Connected += new PviEventHandler(cpu_Connected);
                    cpu.Error += new PviEventHandler(cpu_Error);
                    cpu.Disconnected += new PviEventHandler(cpu_Disconnected);
                }
                cpu.Connect();
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        public void DisconnectCpu(Cpu cpu)
        {
            //if (BendSheetSettings.MockCpuConnection)
            //{
            //    //OnCpuDisconnected(cpu, new PviEventArgs(cpu.Name, cpu.Address, 0, "en-US", BR.AN.PviServices.Action.CpuDisconnect));
            //}
        }

        private void cpu_Connected(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnCpuConnected(sender, e);
            }
        }
        private void OnCpuConnected(object sender, PviEventArgs e)
        {
            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                EventHandler<PviEventArgs> temp = m_CpuConnected;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private void cpu_Disconnected(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnCpuDisconnected(sender, e);
            }
        }

        private void OnCpuDisconnected(object sender, PviEventArgs e)
        {
            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                UnregisterEvents(cpu);
                EventHandler<PviEventArgs> temp = m_CpuDisconnected;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private void cpu_Error(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnCpuError(sender, e);
            }
        }

        private void OnCpuError(object sender, PviEventArgs e)
        {
            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                UnregisterEvents(cpu);
                cpu.Disconnect();
                EventHandler<PviEventArgs> temp = m_CpuError;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        #region Mock Events
        private void cpu_MockConnected(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                OnMockCpuConnected(sender, e);
            }
        }

        private void OnMockCpuConnected(object sender, PviEventArgs e)
        {
            PviEventArgs eventArgs = new PviEventArgs(e.Name, e.Address, 0, "en-US", e.Action);

            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                EventHandler<PviEventArgs> temp = m_CpuConnected;
                if (temp != null)
                {
                    temp(sender, eventArgs);
                }
            }
        }

        private void cpu_MockError(object sender, PviEventArgs e)
        {
            lock (m_EventLock)
            {
                //OnMockCpuError(sender, e);
                OnMockCpuConnected(sender, e);
            }
        }

        private void OnMockCpuError(object sender, PviEventArgs e)
        {
            //Cpu cpu = sender as Cpu;
            //PviEventArgs eventArgs = new PviEventArgs(e.Name, e.Address, 0, "en-US", e.Action);
            //if (cpu != null)
            //{
            //    UnregisterEvents(cpu);
            //    cpu.Disconnect();
            //    EventHandler<PviEventArgs> temp = m_CpuError;
            //    if (temp != null)
            //    {
            //        temp(sender, eventArgs);
            //    }
            //}
        }

        #endregion

        private void UnregisterEvents(Cpu cpu)
        {
            cpu.Connected -= cpu_Connected;
            cpu.Error -= cpu_Error;
            cpu.Disconnected -= cpu_Disconnected;
        }

        private static string CpuName(int destination)
        {
            return String.Concat("PviCpu", destination.ToString("N0", RecipeMasterServices.Format));
        }
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //foreach (Cpu cpu in _cpuList)
                //{
                //    cpu.Disconnect();
                //    cpu.Dispose();
                //}
            }
        }

        ~CpuManager()
        {
            Dispose(false);
        }
        #endregion
    }
}
