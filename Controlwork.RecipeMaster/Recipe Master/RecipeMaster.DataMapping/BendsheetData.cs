using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using BendSheets.Services;
using BendSheets.ConfigurationManagement;

using WH.ComUtils.ExcelManager;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace RecipeMaster.DataMapping
{
    [Serializable]
    //[System.Diagnostics.DebuggerNonUserCode()]
    public class BendSheetData : IDisposable, ICloneable
    {
        public event EventHandler<EventArgs> ExcelClosed;

        private void OnExcelClosed()
        {
            if (ExcelClosed != null)
            {
                ExcelClosed(this, new EventArgs());
            }
        }

        #region Private Members
        private string _customer;
        private string _customerPartNumber;
        private string _customerRevisionLevel;
        private string _notes;
        private string _cablePN;
        private string _cableDescription;
        private string _partNumber;
        private string _asOfDate;
        private string _firstEnd;
        private string _secondEnd;
        private string _firstStrip;
        private string _secondStrip;
        private string _bendRadiusCL;
        private string _firstEndConnector;
        private string _secondEndConnector;
        private string _cableDiameter;
        private string _numberOfCoordinates;
        private string _conversionFactor;
        private string _saveFileName;
        private object[,] _inputData;
        private object[,] _drawingCoordinates;
        private object[,] _autoBendSheet;
        
        [NonSerialized]
        private IExcelEngine _engine;
        private BendSheetDataType _fileType;
        private string _sourceFileName;
        private string _directory;

        #endregion

        #region Public Members

        public string Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public string CustomerPartNumber
        {
            get { return _customerPartNumber; }
            set { _customerPartNumber = value; }
        }

        public string CustomerRevisionLevel
        {
            get { return _customerRevisionLevel; }
            set { _customerRevisionLevel = value; }
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public string CablePN
        {
            get { return _cablePN; }
            set { _cablePN = value; }
        }

        public string CableDescription
        {
            get { return _cableDescription; }
            set { _cableDescription = value; }
        }

        public string PartNumber
        {
            get { return _partNumber; }
            set { _partNumber = value; }
        }

        public string AsOfDate
        {
            get { return _asOfDate; }
            set { _asOfDate = value; }
        }

        public string FirstEnd
        {
            get { return _firstEnd; }
            set { _firstEnd = value; }
        }

        public string SecondEnd
        {
            get { return _secondEnd; }
            set { _secondEnd = value; }
        }

        public string FirstStrip
        {
            get { return _firstStrip; }
            set { _firstStrip = value; }
        }

        public string SecondStrip
        {
            get { return _secondStrip; }
            set { _secondStrip = value; }
        }

        public string BendRadiusCL
        {
            get { return _bendRadiusCL; }
            set { _bendRadiusCL = value; }
        }

        public string FirstEndConnector
        {
            get { return _firstEndConnector; }
            set { _firstEndConnector = value; }
        }

        public string SecondEndConnector
        {
            get { return _secondEndConnector; }
            set { _secondEndConnector = value; }
        }

        public string CableDiameter
        {
            get { return String.IsNullOrEmpty(_cableDiameter) ? null : _cableDiameter;}
            set { _cableDiameter = value; }
        }

        public string NumberOfCoordinates
        {
            get { return _numberOfCoordinates; }
            set { _numberOfCoordinates = value; }
        }

        public string ConversionFactor
        {
            get { return _conversionFactor; }
            set { _conversionFactor = value; }
        }
        public object[,] InputData
        {
            get { return _inputData; }
            set { _inputData = value; }
        }

        public object[,] DrawingCoordinates
        {
            get { return _drawingCoordinates; }
            set { _drawingCoordinates = value; }
        }

        public object[,] AutoBendSheet
        {
            get { return _autoBendSheet; }
            set { _autoBendSheet = value; }
        }

        public bool Visible
        {
            get { return _engine.Visible; }
        }

        public BendSheetDataType FileType
        {
            get { return _fileType; }
        }

        public string FileName
        {
            get { return _sourceFileName; }
        }

        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        public string SaveFileName
        {
            get { return _saveFileName; }
        }


        #endregion

        public BendSheetData() { }

        public BendSheetData(BendSheetDataType fileType)
        {
            _fileType = fileType;
        }

        public BendSheetData(IExcelEngine engine, BendSheetDataType fileType)
        {
            _fileType = fileType;
            _engine = engine;
            LoadSourceFile();
        }

        public BendSheetData(string templateFile, string directory)
        {
            _engine = new ExcelEngine(templateFile);
            _directory = directory;
            LoadSourceFile();
        }

        public BendSheetData(string fileName, BendSheetDataType fileType)
        {
            _sourceFileName = fileName;
            _fileType = fileType;
            _engine = new ExcelEngine(fileName);
            LoadSourceFile();
        }

        public BendSheetData(string sourceFile, string targetFile, BendSheetDataType fileType)
        {
            _sourceFileName = sourceFile;
            _fileType = fileType;

            using (_engine = new ExcelEngine(sourceFile))
            {
                LoadSourceFile();
                _engine.CloseExcelWorkbook();
            }

            _engine = new ExcelEngine(targetFile);
            WriteTargetFile();
            Caption();
        }

        private void Caption()
        {
            string caption = String.Concat(_fileType.ToString(), " FILE    -- Customer: ", _customer, "    -- Part Number: ", _partNumber.PadRight(40));
            _engine.SetCaption(caption);
        }

        private void LoadSourceFile()
        {
            _partNumber = GetStringCell(BendSheetDataMap.PART_NUMBER);
            _inputData = _engine.GetRange(BendSheetDataMap.INPUT_DATA[0], BendSheetDataMap.INPUT_DATA[1]);
            _drawingCoordinates = _engine.GetRange(BendSheetDataMap.DRAWING_COORDINATES[0], BendSheetDataMap.DRAWING_COORDINATES[1]);
            _autoBendSheet = _engine.GetRange(BendSheetDataMap.AUTO_BEND_SHEET[0], BendSheetDataMap.AUTO_BEND_SHEET[1]);
            _customer = GetStringCell(BendSheetDataMap.CUSTOMER);
            _customerPartNumber = GetStringCell(BendSheetDataMap.CUSTOMER_PART_NUMBER);
            _customerRevisionLevel = GetStringCell(BendSheetDataMap.CUSTOMER_REVISION_LEVEL);
            _notes = GetStringCell(BendSheetDataMap.NOTES);
            _cablePN = GetStringCell(BendSheetDataMap.CABLE_PN);
            _cableDescription = GetStringCell(BendSheetDataMap.CABLE_DESCRIPTION);
            _asOfDate = GetStringCell(BendSheetDataMap.AS_OF_DATE);
            _firstEnd = GetStringCell(BendSheetDataMap.FIRST_END);
            _secondEnd = GetStringCell(BendSheetDataMap.SECOND_END);
            _firstStrip = GetStringCell(BendSheetDataMap.FIRST_STRIP);
            _secondStrip = GetStringCell(BendSheetDataMap.SECOND_STRIP);
            _bendRadiusCL = GetStringCell(BendSheetDataMap.BEND_RADIUS_CL);
            _firstEndConnector = GetStringCell(BendSheetDataMap.FIRST_END_CONNECTOR);
            _secondEndConnector = GetStringCell(BendSheetDataMap.SECOND_END_CONNECTOR);
            _cableDiameter = GetStringCell(BendSheetDataMap.CABLE_DIAMETER);
            _numberOfCoordinates = GetStringCell(BendSheetDataMap.NUMBER_OF_COORDINATES);
            _conversionFactor = GetStringCell(BendSheetDataMap.CONVERSION_FACTOR);
        }

        private void WriteTargetFile()
        {
            _engine.WriteRange(BendSheetDataMap.INPUT_DATA[0], BendSheetDataMap.INPUT_DATA[1],_inputData);
            _engine.WriteRange(BendSheetDataMap.DRAWING_COORDINATES[0], BendSheetDataMap.DRAWING_COORDINATES[1], _drawingCoordinates);
            _engine.WriteRange(BendSheetDataMap.AUTO_BEND_SHEET[0], BendSheetDataMap.AUTO_BEND_SHEET[1], _autoBendSheet);
            _engine.WriteCellValue(BendSheetDataMap.PART_NUMBER, _partNumber);
            _engine.WriteCellValue(BendSheetDataMap.CUSTOMER, _customer);
            _engine.WriteCellValue(BendSheetDataMap.CUSTOMER_PART_NUMBER, _customerPartNumber);
            _engine.WriteCellValue(BendSheetDataMap.CUSTOMER_REVISION_LEVEL, _customerRevisionLevel);
            _engine.WriteCellValue(BendSheetDataMap.NOTES, _notes);
            _engine.WriteCellValue(BendSheetDataMap.CABLE_PN, _cablePN);
            _engine.WriteCellValue(BendSheetDataMap.CABLE_DESCRIPTION, _cableDescription);
            _engine.WriteCellValue(BendSheetDataMap.AS_OF_DATE, _asOfDate);
            _engine.WriteCellValue(BendSheetDataMap.FIRST_END, _firstEnd);
            _engine.WriteCellValue(BendSheetDataMap.SECOND_END, _secondEnd);
            _engine.WriteCellValue(BendSheetDataMap.FIRST_STRIP, _firstStrip);
            _engine.WriteCellValue(BendSheetDataMap.SECOND_STRIP, _secondStrip);
            _engine.WriteCellValue(BendSheetDataMap.BEND_RADIUS_CL, _bendRadiusCL);
            _engine.WriteCellValue(BendSheetDataMap.FIRST_END_CONNECTOR, _firstEndConnector);
            _engine.WriteCellValue(BendSheetDataMap.SECOND_END_CONNECTOR, _secondEndConnector);
            _engine.WriteCellValue(BendSheetDataMap.CABLE_DIAMETER, _cableDiameter);
            _engine.WriteCellValue(BendSheetDataMap.NUMBER_OF_COORDINATES, _numberOfCoordinates);
            _engine.WriteCellValue(BendSheetDataMap.CONVERSION_FACTOR, _conversionFactor);
        }

        private void SetSaveFileName()
        {
            _saveFileName = GetStringCell(BendSheetSettings.SavePathCell);
        }

        public object Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            BendSheetData copy = (BendSheetData)bf.Deserialize(ms);
            ms.Close();
            return copy;
        }

        private string GetStringCell(string map)
        {
            object returnObject = _engine.GetCellValue(map);
            if (returnObject == null)
            {
                return String.Empty;
            }
            return returnObject.ToString();
        }

        public void ShowExcel(bool visible)
        {
            if (_engine != null)
            {
                _engine.SetVisibleTo(visible);
            }
        }

        public void CloseExcel()
        {
            _engine.SetVisibleTo(false);
            _engine.CloseExcelWorkbook();
            _engine.Dispose();
            OnExcelClosed();
        }

        public void Save()
        {
            WriteTargetFile();
            SetSaveFileName();
            string fileName = BendSheetsServices.BuildFileName(_saveFileName, Directory);
            if (BendSheetSettings.Archive)
            {
                BendSheetsServices.RenameExistingFile(fileName);
            }
            else
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }

            Array.Clear(_inputData, 1, _inputData.Length);
            _engine.WriteRange(BendSheetDataMap.INPUT_DATA[0], BendSheetDataMap.INPUT_DATA[1], _inputData);

            Array.Clear(_drawingCoordinates, 1, _drawingCoordinates.Length);
            _engine.WriteRange(BendSheetDataMap.DRAWING_COORDINATES[0], BendSheetDataMap.DRAWING_COORDINATES[1], _drawingCoordinates);

            _engine.Save(fileName);
            CloseExcel();
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
                if (_engine != null)
                {
                    _engine.Dispose();
                }
            }
        }

        #endregion
    }

    public enum BendSheetDataType
    {
        MASTER, PRODUCTION, UNKNOWN
    };
}
