using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace RecipeMaster.DataMapping
{
    public static class BendSheetDataUtils
    {
        public static int CountRows(object[,] data)
        {
            int count = 0;
            bool valid = true;

            for (int i = data.GetLowerBound(0); i <= data.GetUpperBound(0); i++)
            {
                if (ParseNumberFromString(ObjectToString(data[i, 1])).HasValue)
                {
                    if (valid)
                    {
                        count++;
                    }
                }
                else
                {
                    valid = false;
                }
            }
            return count;
        }

        public static string ObjectToString(object o)
        {
            if (o == null) return String.Empty;
            return o.ToString();
        }

        public static int? ParseNumberFromString(string str)
        {
            if (str == null) return null;

            if (str.Equals(String.Empty)) return null;

            str = str.Trim();
            Match m = Regex.Match(str, @"(?<number>\d+)");

            int retValue;

            if (Int32.TryParse(m.Value, out retValue))
            {
                return retValue;
            }
            return null;
        }

        public static bool ValidRow(object[] data)
        {
            return Array.Exists(data, delegate(object o) { return o != null; });
        }

        public static object[] CreateRow(object[,] data, int rowNum)
        {
            int length = data.GetLength(data.Rank-1);
            List<object> list = new List<object>(length);
            for (int i = 0; i < length; i++)
            {
                list.Add(data[rowNum, i]);
            }
            object[] o = list.ToArray();
            return o;
        }

        public static object[,] ParseData(object[,] data)
        {
            object[,] oArrayMulti = null;
            int valid =  CountRows(data);

            for (int i = 0; i < valid; i++)
            {
                oArrayMulti = AddRowToMulti(CreateRow(data, i), oArrayMulti);
            }

            return oArrayMulti;
        }

        public static object[,] AddRowToMulti(object[] o, object[,] oArrayMulti)
        {
            if (oArrayMulti == null && o != null)
            {
                return CreateMulti(o);
            }

            if (o.Length <= 0 || o.Length - 1 != oArrayMulti.GetUpperBound(oArrayMulti.Rank - 1)) return null;

            Array retArrayMulti = Array.CreateInstance(typeof(object), oArrayMulti.GetUpperBound(0) + 2, oArrayMulti.GetUpperBound(1) + 1);
            Array.Copy(oArrayMulti, retArrayMulti, oArrayMulti.Length);

            for (int i = 0; i < o.Length; i++)
            {
                retArrayMulti.SetValue(o[i], oArrayMulti.GetUpperBound(0) + 1, i );
            }
            return (object[,])retArrayMulti;
        }

        public static object[,] CreateMulti(object[] oArray)
        {
            if (oArray == null || oArray.Length < 1) return null;

            Array retArrayMulti = Array.CreateInstance(typeof(object), 1, oArray.Length);

            for (int i = 0; i < oArray.Length; i++)
            {
                retArrayMulti.SetValue(oArray[i], 0, i);
            }

            return (object[,])retArrayMulti;
        }

        public static object[,] CreateFullArray(object[,] array, int rows)
        {
            object[] o = (object[])Array.CreateInstance(typeof(object), array.GetUpperBound(1) + 1);

            object[,] validArray = ParseData(array);

            for (int i = validArray.GetUpperBound(0) + 1 ; i < rows; i++)
            {
                validArray = AddRowToMulti(o, validArray);
            }
            return validArray;
        }
    }
}
