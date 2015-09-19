using System;
using System.Collections.Generic;
using System.Text;

namespace BendSheets.PVICommunication
{
    public class CpuData
    {
        private int _destinationStation;
        private int _sourceStation;
        private string _cpuName;

        public int DestinationStation
        {
            get { return _destinationStation; }
        }

        public int SourceStation
        {
            get { return _sourceStation; }
        }

        public string CpuName
        {
            get { return _cpuName; }
        }

        public CpuData(int destinationStation, int sourceStation, string cpuName)
        {
            _destinationStation = destinationStation;
            _sourceStation = sourceStation;
            _cpuName = cpuName;
        }
    }
}
