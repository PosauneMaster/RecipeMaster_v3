using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using RecipeMaster.Services;
using BendSheets.ConfigurationManagement;

using WH.ComUtils.ExcelManager;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BendSheets
{
    public class RecipeData 
    {
        private static object m_Locker = new object();

        public static VariableCollections CreateVariableList(IEnumerable<RecipeTemplateItem> items)
        {
            List<RecipeVariable> receiveVariableList = new List<RecipeVariable>();
            List<RecipeVariable> sendVariableList = new List<RecipeVariable>();

            foreach (RecipeTemplateItem item in items)
            {
                if (item.ItemType == RecipeTemplateItemType.Cell)
                {
                    receiveVariableList.Add(new RecipeVariable(item.ReceiveName) { CellMap = item.CellStart});
                    sendVariableList.Add(new RecipeVariable(item.SendName) { CellMap = item.CellStart });
                }

                if (item.ItemType == RecipeTemplateItemType.Range)
                {
                    string[] splitter = item.ReceiveName.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] start = Regex.Split(item.CellStart, @"[^\d]");
                    string[] end = Regex.Split(item.CellEnd, @"[^\d]");
                    string column = Regex.Replace(item.CellStart, @"\d", "");


                    Int32 startCell;
                    Int32 endCell;

                    if (start.Length == 2 && end.Length == 2 && column.Length > 0)
                    {
                        if (Int32.TryParse(start[1], out startCell) && Int32.TryParse(end[1], out endCell))
                        {
                            for (Int32 i = 0; startCell < endCell + 1; startCell++, i++)
                            {
                                string variableName = CreateArrayVariable(item, i);
                                string cellMap = column + startCell.ToString();
                                receiveVariableList.Add(new RecipeVariable(variableName){CellMap = cellMap});
                                sendVariableList.Add(new RecipeVariable(variableName) { CellMap = cellMap });
                            }
                        }
                    }
                }
            }
            return new VariableCollections(sendVariableList, receiveVariableList);
        }

        private static string CreateArrayVariable(RecipeTemplateItem item, Int32 i)
        {
            return item.ReceiveName.Insert(ArrayIndexPosition(item.ReceiveName), i.ToString("N0"));
        }

        private static int ArrayIndexPosition(string variableName)
        {
            return variableName.IndexOf("[") + 1;
        }

        public static void MapDataFromExcel(string fileName, IEnumerable<RecipeVariable> itemList)
        {
            using (IExcelEngine excelEngine = new ExcelEngine(fileName))
            {
                foreach (RecipeVariable rv in itemList)
                {
                    rv.Value = GetCell(rv.CellMap, excelEngine);
                }
            }
        }

        public static VariableCollections MapDataFromExcel(string fileName, IList<RecipeTemplateItem> items)
        {
            List<RecipeVariable> sendVariableList = new List<RecipeVariable>();
            List<RecipeVariable> receiveVariableList = new List<RecipeVariable>();
            Dictionary<string, object[]> rangeData = new Dictionary<string, object[]>();

            using (IExcelEngine excelEngine = new ExcelEngine(fileName))
            {
                foreach (RecipeTemplateItem item in items)
                {
                    if (item.ItemType == RecipeTemplateItemType.Cell)
                    {
                        object data = GetCell(item.CellStart, excelEngine);
                        sendVariableList.Add(new RecipeVariable(item.SendName, data));
                        receiveVariableList.Add(new RecipeVariable(item.ReceiveName));
                    }

                    if (item.ItemType == RecipeTemplateItemType.Range)
                    {
                        object[] dataRange = GetRange(item.CellStart, item.CellEnd, excelEngine);
                        rangeData.Add(item.SendName, dataRange);
                    }
                }

                foreach (KeyValuePair<string, object[]> kvp in rangeData)
                {
                    sendVariableList.AddRange(MapRangeData(kvp.Key, kvp.Value));
                }
            }

            return new VariableCollections(sendVariableList, receiveVariableList);
        }

        private static object GetCell(string map, IExcelEngine excelEngine)
        {
            object returnObject = excelEngine.GetCellValue(map);
            if (returnObject == null)
            {
                return new object();
            }
            return returnObject;
        }

        private static object[] GetRange(string cellStart, string cellEnd, IExcelEngine excelEngine)
        {
            object[,] oArray = excelEngine.GetRange(cellStart, cellEnd);
            return ExcelUtils.GetFirstRankArray(oArray);
        }

        private static Collection<RecipeVariable> MapRangeData(string name , object[] rangeData)
        {
            Collection<RecipeVariable> collection = new Collection<RecipeVariable>();

            string[] splitter = name.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rangeData.Length; i++)
            {
                string variableName = String.Format("{0}[{1}]{2}", splitter[0], i.ToString("N0"), splitter[1]);
                collection.Add(new RecipeVariable(variableName, rangeData[i]));
            }
            return collection;
        }

        public static void WriteExcelData(string destinationFile, RecipeTemplates templates, IEnumerable<RecipeVariable> variableList)
        {
            try
            {
                using (IExcelEngine excelEngine = new ExcelEngine(destinationFile))
                {
                    foreach (RecipeVariable rv in variableList)
                    {
                        excelEngine.WriteCellValue(rv.CellMap, rv.Value);
                    }
                    excelEngine.Save(destinationFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public static void WriteExcelData(string tempFile, string destinationFile, RecipeTemplates templates, IEnumerable<RecipeVariable> variableList)
        {
            try
            {
                RecipeMasterServices.WriteNewProductionFile(tempFile);

                using (IExcelEngine excelEngine = new ExcelEngine(tempFile))
                {
                    foreach (RecipeVariable rv in variableList)
                    {
                        excelEngine.WriteCellValue(rv.CellMap, rv.Value);
                    }
                    RecipeMasterServices.RenameExistingFile(destinationFile, false);
                    excelEngine.Save(destinationFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }

    public enum RecipeSheetDataType
    {
        Master,
        Production
    }
}
