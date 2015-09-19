using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using RecipeMaster.DataMapping;
using BR.AN.PviServices;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace BendSheets.PVICommunication
{
    public class PVIEngine : IDisposable
    {
        #region Cpu Events

        private EventHandler<PVIEngineEventArgs> m_CpuConnected;
        private EventHandler<PVIEngineEventArgs> m_CpuDisconnected;
        private EventHandler<PVIEngineEventArgs> m_CpuError;

        public event EventHandler<PVIEngineEventArgs> CpuConnected
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

        private void OnCpuConnected(object sender, PviEventArgs e)
        {
            EventHandler<PVIEngineEventArgs> temp = this.m_CpuConnected;
            if (temp != null)
            {
                Cpu cpu = sender as Cpu;
                CpuData data = new CpuData((int)cpu.Connection.TcpIp.DestinationStation, (int)cpu.Connection.TcpIp.SourceStation, cpu.Name);
                PVIEngineEventArgs eventArgs = new PVIEngineEventArgs(e, data);
                temp(sender, eventArgs);
            }
        }

        public event EventHandler<PVIEngineEventArgs> CpuDisconnected
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

        private void OnCpuDisconnected(object sender, PviEventArgs e)
        {
            EventHandler<PVIEngineEventArgs> temp = this.m_CpuDisconnected;
            if (temp != null)
            {
                Cpu cpu = sender as Cpu;
                CpuData data = new CpuData((int)cpu.Connection.TcpIp.DestinationStation, (int)cpu.Connection.TcpIp.SourceStation, cpu.Name);
                PVIEngineEventArgs eventArgs = new PVIEngineEventArgs(e, data);
                temp(sender, eventArgs);
            }
        }

        public event EventHandler<PVIEngineEventArgs> CpuError
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

        private void OnCpuError(object sender, PviEventArgs e)
        {
            EventHandler<PVIEngineEventArgs> temp = this.m_CpuError;
            if (temp != null)
            {
                Cpu cpu = sender as Cpu;
                CpuData data = new CpuData((int)cpu.Connection.TcpIp.DestinationStation, (int)cpu.Connection.TcpIp.SourceStation, cpu.Name);
                PVIEngineEventArgs eventArgs = new PVIEngineEventArgs(e, data);
                temp(sender, eventArgs);
            }
        }

        #endregion

        #region Private Members

        private PviEventArgs m_PVIEventArgs;
        private CpuManager m_CpuManager;
        //private VariableManager m_VariableManager;
        private List<int> m_activeDestinations = new List<int>();

        public event EventHandler<PviEventArgs> VariableChanged;
        public event EventHandler<GetDataEventArgs> GetData;
        public event EventHandler<GetDataEventArgs> SendProductionData;

        private readonly object m_EventLock = new Object();

        #endregion


        public PviEventArgs PVIEventArgs
        {
            get { return m_PVIEventArgs; }
            set { m_PVIEventArgs = value; }
        }

        private PVIEngine()
        {
            m_CpuManager = new CpuManager();
            m_CpuManager.CpuConnected += new EventHandler<PviEventArgs>(m_CpuManager_CpuConnected);
            m_CpuManager.CpuDisconnected += new EventHandler<PviEventArgs>(m_CpuManager_CpuDisconnected);
            m_CpuManager.CpuError += new EventHandler<PviEventArgs>(m_CpuManager_CpuError);

            //m_VariableManager = new VariableManager();
            //m_VariableManager.VariableChanged += new EventHandler<PviEventArgs>(_variableManager_VariableChanged);
            //m_VariableManager.GetData += new EventHandler<GetDataEventArgs>(variableManager_GetData);
            //m_VariableManager.SendProductionData += new EventHandler<GetDataEventArgs>(variableManager_SendProductionData);
        }
        
        #region Singleton Factory

        private static object _threadLock = new object();
        private static PVIEngine pviEngine = new PVIEngine();

        public static PVIEngine PviEngineInstance
        {
            get
            {
                if (pviEngine == null)
                {
                    lock (_threadLock)
                    {
                        pviEngine = new PVIEngine();
                    }
                }
                return pviEngine;
            }
        }

        #endregion

        #region CpuManager event handlers
        private void m_CpuManager_CpuError(object sender, PviEventArgs e)
        {
            if (e.ErrorCode != 0)
            {
                OnCpuError(sender, e);
            }
        }

        private void m_CpuManager_CpuDisconnected(object sender, PviEventArgs e)
        {
            OnCpuDisconnected(sender, e);
        }

        private void m_CpuManager_CpuConnected(object sender, PviEventArgs e)
        {
            Cpu cpu = sender as Cpu;
            ConnectVariable(cpu);
            OnCpuConnected(sender, e);
        }

        public void ConnectCpu(int destination, int SourceStationId)
        {
            PviService pviService = PviService.PviServiceInstance;
            //m_CpuManager.ConnectCpu(pviService.Service, destination, (byte)SourceStationId);
        }

        public void DisconnectCpu(int destination)
        {
            PviService pviService = PviService.PviServiceInstance;
            //m_CpuManager.DisconnectCpu(pviService.Service, destination);
        }

        #endregion

        #region VariableManager event handlers

        private void variableManager_SendProductionData(object sender, GetDataEventArgs e)
        {
            OnSendProductionData(sender as Variable, e);
        }

        private void variableManager_GetData(object sender, GetDataEventArgs e)
        {
            Variable v = sender as Variable;
            CpuData cpuData = e.CpuInfo;
            string partNumber = ReadPartNumber(cpuData.DestinationStation);
            //BendSheetDataType dataType = GetDataType(v);
            //OnGetData(v, new GetDataEventArgs(partNumber, dataType, cpuData));
        }

        private void ConnectVariable(Cpu cpu)
        {
            //m_VariableManager.LoadVariables(cpu);
        }

        #endregion

        private void OnSendProductionData(Variable variable, GetDataEventArgs e)
        {
            if (SendProductionData != null)
            {
                SendProductionData(variable, e);
            }
        }

        private void OnGetData(Variable v, GetDataEventArgs e)
        {
            if (GetData != null)
            {
                GetData(v, e);
            }
        }

        //public void SaveProductionData(string templateFile, string saveDirectory, int destination)
        //{
        //    BendSheetData bendData = new BendSheetData(templateFile, saveDirectory);
        //    //RunningPart.MapBendSheetData(m_CpuManager[destination], bendData);
        //    try
        //    {
        //        bendData.Save();
        //        bendData.Dispose();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        bendData.Dispose();
        //    }
        //}

        //public void Send(BendSheetData data, int destination)
        //{
        //    CurrentPart cp = new CurrentPart();
        //    //cp.MapVariableData(m_CpuManager[destination], data);
        //}

        public void FileNotFound(int destination)
        {
            //CurrentPart cp = new CurrentPart();
            //cp.FileNotFound(m_CpuManager[destination]);
        }

        //private static BendSheetDataType GetDataType(Variable v)
        //{
        //    if (v.UserTag.Equals(ControllerVariables.BTN_GET_MASTER_DATA))
        //    {
        //        return BendSheetDataType.MASTER;
        //    }
        //    else if (v.UserTag.Equals(ControllerVariables.BTN_GET_PRODUCTION_DATA))
        //    {
        //        return BendSheetDataType.PRODUCTION;
        //    }
        //    else
        //    {
        //        return BendSheetDataType.UNKNOWN;
        //    }
        //}

        private void _variableManager_VariableChanged(object o, PviEventArgs e)
        {
            OnVariableChanged(o, e);
        }

        private void OnVariableChanged(object sender, PviEventArgs e)
        {
            if (VariableChanged != null)
            {
                VariableChanged(sender, e);
            }
        }

        private string ReadPartNumber(int destination)
        {
            //Variable v = m_CpuManager[destination].Variables[CurrentPartVariables.CUSTOMER_PN];
            //v.ReadValue();
            //return v.Value.ToString();
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_CpuManager != null)
                {
                    m_CpuManager.Dispose();
                }
            }
        }

        #endregion
    }

    #region PVIEngineEventArgs
    public class PVIEngineEventArgs : EventArgs
    {
        private CpuData m_CpuData;
        private PviEventArgs m_PviEventArgs;

        public PviEventArgs PviEventArgs
        {
            get { return m_PviEventArgs; }
            set { m_PviEventArgs = value; }
        }

        public CpuData CpuData
        {
            get { return m_CpuData; }
            set { m_CpuData = value; }
        }


        public PVIEngineEventArgs(PviEventArgs e, CpuData data)
        {
            this.m_PviEventArgs = e;
            this.m_CpuData = data;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Cpu Name = " + m_CpuData.CpuName);
            builder.AppendLine("Cpu Destination = " + m_CpuData.DestinationStation.ToString());
            builder.AppendLine("Cpu Source Station = " + m_CpuData.SourceStation.ToString());
            builder.AppendLine("Action = " + PviEventArgs.Action.ToString());
            builder.AppendLine("Error Code = " + PviEventArgs.ErrorCode.ToString());
            builder.AppendLine("Error Text = " + PviEventArgs.ErrorText);
            builder.AppendLine();
            return builder.ToString();
        }
    }
    #endregion

    #region GetDataEventArgs
    public class GetDataEventArgs : EventArgs
    {
        private string _partNumber;
        //private BendSheetDataType _dataType;
        private CpuData _cpuInfo;
        private int _destination;

        public int Destination
        {
          get { return _destination; }
        }

        public string PartNumber
        {
            get { return _partNumber; }
        }

        //public BendSheetDataType DataType
        //{
        //    get { return _dataType; }
        //}

        public CpuData CpuInfo
        {
            get { return _cpuInfo; }
        }

        public GetDataEventArgs(CpuData cpuData)
        {
            _partNumber = String.Empty;
            //_dataType = BendSheetDataType.UNKNOWN;
            if (cpuData != null)
            {
                _destination = cpuData.DestinationStation;
                _cpuInfo = cpuData;
            }
        }

        //public GetDataEventArgs(string partNumber, BendSheetDataType dataType, CpuData cpuData)
        //{
        //    _partNumber = partNumber;
        //    _dataType = dataType;
        //    if (cpuData != null)
        //    {
        //        _destination = cpuData.DestinationStation;
        //        _cpuInfo = cpuData;
        //    }
        //}

        //public GetDataEventArgs(BendSheetDataType dataType, CpuData cpuData)
        //{
        //    _partNumber = String.Empty;
        //    _dataType = dataType;
        //    if (cpuData != null)
        //    {
        //        _destination = cpuData.DestinationStation;
        //        _cpuInfo = cpuData;
        //    }
        //}
    }
    #endregion

}
