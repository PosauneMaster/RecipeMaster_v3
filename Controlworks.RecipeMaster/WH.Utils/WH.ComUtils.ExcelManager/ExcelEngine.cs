using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Security;
using System.Security.Permissions;

[assembly: CLSCompliant(false)]

namespace WH.ComUtils.ExcelManager
{
    //
    //Binding for Office automation servers with Visual C# .NET
    //http://support.microsoft.com/default.aspx?scid=kb;en-us;Q302902
    //
    //[System.Diagnostics.DebuggerNonUserCode()]
    public class ExcelEngine : IExcelEngine, IDisposable
    {
        private const string EXCEL_APPLICATION = "Excel.Application";

        #region Private Members
        private object _excelApplication;       //COM object
        private object _excelWorkbooks;         //COM object
        private object _excelWorkbook;          //COM object

        private object _excelWorksheets;        //COM Object
        private object _excelActiveWorksheet;   //COM Object
        private object _excelRange;             //COM Object

        private IntPtr _handle;
        private bool _disposed;
        private bool _visible;
        private object _version;
        private CultureInfo _culture;
        #endregion

        #region Public Members

        //http://msdn2.microsoft.com/en-us/library/microsoft.office.interop.excel(VS.80).aspx
        public object ExcelApplication
        {
            get { return _excelApplication; }
        }
        public object ExcelWorkbook
        {
            get { return _excelWorkbooks; }
        }
        public bool Visible
        {
            get { return _visible; }
        }
        public string Version
        {
            get { return _version.ToString(); }
        }
        #endregion

        #region Constructors and Initialize methods
        public ExcelEngine()
        {
            InitializeExcelEngine();
        }
        public ExcelEngine(string path)
        {
            InitializeExcelEngine();
            GetExcelWorkbook(path);
            GetExcelWorksheets();
        }
        private void InitializeExcelEngine()
        {
            SetCulture();
            CreateExcelApplication();
            SetVisibleTo(false);
            _handle = GetApplicationHandle();

            _version = _excelApplication.GetType().InvokeMember("Version", BindingFlags.GetProperty, null, _excelApplication, null, _culture);
        }

        private void SetCulture()
        {
            _culture = new CultureInfo("en-US", true);
        }
        #endregion

        #region Public Methods

        public static bool ExcelInstalled()
        {
            return ApplicationEngine.ApplicationInstalled(EXCEL_APPLICATION);
        }

        public void CreateExcelApplication()
        {
            _excelApplication = ApplicationEngine.CreateApplication(EXCEL_APPLICATION);
        }

        //http://msdn2.microsoft.com/library/microsoft.office.interop.excel.applicationclass.visible(VS.80).aspx
        public void SetVisibleTo(bool visible)
        {
            object[] parameter = new object[1];
            parameter[0] = visible;

            _excelApplication.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, _excelApplication, parameter, _culture);

            _visible = visible;
        }

        //http://support.microsoft.com/kb/213428/
        public void ShowSavePrompt(bool suppress)
        {
            if (_excelApplication != null)
            {
                object[] parameter = new object[1];
                parameter[0] = suppress;

                _excelApplication.GetType().InvokeMember("DisplayAlerts", BindingFlags.SetProperty, null, _excelApplication, parameter, _culture);
            }
        }

        //http://msdn2.microsoft.com/library/microsoft.office.interop.excel.workbooks.open(VS.80).aspx
        public void GetExcelWorkbook(string path)
        {
            object[] parameters = new object[13] 
            {
                path,           //Filename
                0,              //UpdateLinks
                false,          //ReadOnly
                5,              //Format
                String.Empty,   //Password
                String.Empty,   //WriteResPassword
                true,           //IgnoreReadOnlyRecommended
                Type.Missing,   //Origin
                "\t",           //Delimiter
                true,           //Editable
                false,          //Notify
                0,              //Converter
                true            //AddToMru
            };

            object[] parms = new object[1];
            parms[0] = false;

            _excelApplication.GetType().InvokeMember("EnableEvents", BindingFlags.SetProperty, null, _excelApplication, parms, _culture);

            _excelWorkbooks = _excelApplication.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, _excelApplication, null, _culture);

            _excelWorkbook = _excelWorkbooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, _excelWorkbooks, parameters, _culture);

            GetExcelWorksheets();

            Protect();
        }

        public void SetCaption(string caption)
        {
            object[] title = new object[1];
            title[0] = caption;

            _excelApplication.GetType().InvokeMember("Caption", BindingFlags.SetProperty, null, _excelApplication, title, _culture);
        }

        public void Protect()
        {
            if (_excelActiveWorksheet != null)
            {
                _excelActiveWorksheet.GetType().InvokeMember("Protect", BindingFlags.InvokeMethod, null, _excelActiveWorksheet, null, _culture);
            }
        }

        public void Unprotect()
        {
            //if (_excelActiveWorksheet != null)
            //{
            //    _excelActiveWorksheet.GetType().InvokeMember("Unprotect", BindingFlags.InvokeMethod, null, _excelActiveWorksheet, null, _culture);
            //}
        }

        public void CloseExcelWorkbook()
        {
            ShowSavePrompt(false);

            _excelWorkbooks.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _excelWorkbooks, null, _culture);
        }

        //http://msdn2.microsoft.com/en-us/library/microsoft.office.interop.excel.sheets_members(VS.80).aspx
        public void GetExcelWorksheets()
        {
            _excelWorksheets = _excelWorkbook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, _excelWorkbook, null, _culture);

            object[] parameter = new object[1];
            parameter[0] = 1;

            _excelActiveWorksheet = _excelWorksheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, _excelWorksheets, parameter, _culture);
        }

        //http://msdn2.microsoft.com/en-us/library/microsoft.office.interop.excel.range_members(VS.80).aspx
        public object[,] GetRange(string startCell, string endCell)
        {
            if (!ExcelUtils.ValidCell(startCell))
                return null;
            if (!ExcelUtils.ValidCell(endCell))
                return null;

            object[] parameters = new object[2];
            parameters[0] = startCell;
            parameters[1] = endCell;

            _excelRange = _excelActiveWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, _excelActiveWorksheet, parameters, _culture);
            object[,] range = (object[,])_excelRange.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, _excelRange, null, _culture);

            return range;
        }

        public void WriteRange(string startCell, string endCell, object[,] values)
        {
            //Unprotect();

            object[] parameters = new object[2];
            parameters[0] = startCell;
            parameters[1] = endCell;

            _excelRange = _excelActiveWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, _excelActiveWorksheet, parameters, _culture);

            object[] parms = new object[1];
            parms[0] = values;

            _excelRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, _excelRange, parms, _culture);

            Protect();
        }

        public void Save(string fileName)
        {
           //Unprotect();
            ShowSavePrompt(false);

            object[] parameters = new object[2];
            parameters[0] = true;
            parameters[1] = fileName;

            _excelWorkbook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _excelWorkbook, parameters, _culture);
        }

        public object GetCellValue(string cell)
        {
            if (!ExcelUtils.ValidCell(cell))
                return String.Empty;

            object[] parameters = new object[2];
            parameters[0] = cell;
            parameters[1] = Missing.Value;

            _excelRange = _excelActiveWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, _excelActiveWorksheet, parameters, _culture);
            return _excelRange.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, _excelRange, null, _culture);
        }

        public void WriteCellValue(string cell, object parameter)
        {
            //Unprotect();

            if (!ExcelUtils.ValidCell(cell)) return;

            object[] cells = new object[2];
            cells[0] = cell;
            cells[1] = Missing.Value;

            _excelRange = _excelActiveWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, _excelActiveWorksheet, cells, _culture);

            object[] parameters = new object[1];
            parameters[0] = parameter;
            _excelRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, _excelRange, parameters, _culture);

            Protect();
        }

        #endregion

        #region Private Methods

        private IntPtr GetApplicationHandle()
        {
            return (IntPtr)((Int32)_excelApplication.GetType().InvokeMember("Hwnd", BindingFlags.GetProperty, null, _excelApplication, null, _culture));
        }

        #endregion

        #region IDispose logic
        // Necessary for unmanaged calls
        //http://msdn2.microsoft.com/en-us/library/b1yfkh5e(VS.71).aspx
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Close the Workbook first or there can be problems
                if (_excelWorkbooks != null)
                {
                    if (disposing)
                    {
                        CloseExcelWorkbook();
                    }
                    Marshal.FinalReleaseComObject(_excelWorkbooks);
                    _excelWorkbooks = null;
                }
                if (_excelWorksheets != null)
                {
                    Marshal.FinalReleaseComObject(_excelWorksheets);
                    _excelWorksheets = null;
                }
                if (_excelActiveWorksheet != null)
                {
                    Marshal.FinalReleaseComObject(_excelActiveWorksheet);
                    _excelActiveWorksheet = null;
                }
                if (_excelRange != null)
                {
                    Marshal.FinalReleaseComObject(_excelRange);
                    _excelRange = null;
                }
                if (_excelApplication != null)
                {
                    Marshal.FinalReleaseComObject(_excelApplication);
                    _excelApplication = null;
                }
                NativeMethods.CloseCOMHandle(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        ~ExcelEngine()
        {
            Dispose(false);
        }
        #endregion
    }
}
