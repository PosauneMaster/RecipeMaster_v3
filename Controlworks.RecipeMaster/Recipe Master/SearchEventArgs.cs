using System;
using System.Collections.Generic;
using System.Text;
using RecipeMaster.DataMapping;

namespace BendSheets
{
    public class SearchEventArgs : EventArgs
    {
        private string _partNumber;
        private string _directory;
        private BendSheetDataType _dataType;

        public string PartNumber
        {
            get { return _partNumber; }
        }

        public string Directory
        {
            get { return _directory; }
        }

        public BendSheetDataType DataType
        {
            get { return _dataType; }
        }

        public SearchEventArgs(string partNumber, string directory, BendSheetDataType dataType)
        {
            _partNumber = partNumber;
            _directory = directory;
            _dataType = dataType;
        }
    }
}
