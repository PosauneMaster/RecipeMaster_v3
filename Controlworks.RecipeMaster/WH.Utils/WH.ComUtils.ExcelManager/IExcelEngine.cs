using System;
using System.IO;
using System.Collections.Generic;

namespace WH.ComUtils.ExcelManager
{ 
    public interface IExcelEngine : IDisposable
    {
        bool Visible { get; }

        object[,] GetRange(string startCell, string endCell);
        object GetCellValue(string cell);
        void WriteRange(string startCell, string endCell, object[,] values);
        void WriteCellValue(string cell, object parameter);
        void SetCaption(string caption);

        void SetVisibleTo(bool visible);
        void CloseExcelWorkbook();
        void Save(string fileName);
    }
}
