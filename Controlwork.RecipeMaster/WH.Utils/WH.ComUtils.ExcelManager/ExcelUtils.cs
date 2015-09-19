using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WH.ComUtils.ExcelManager
{
    //[System.Diagnostics.DebuggerNonUserCode()]
    public static class ExcelUtils
    {
        public static bool ValidColumnAddress(string address)
        {
            if (String.IsNullOrEmpty(address))
                return false;

            if (address.Length > 2)
                return false;

            if (!Regex.IsMatch(address, @"^[A-Z]+$"))
                return false;

            if (address.Length == 1)
                return address.CompareTo("Z") <= 0;

            if (address.Length == 2)
                return address.CompareTo("IV") <= 0;

            return false;
        }

        public static bool ValidRowAddress(string address)
        {
            if (address == null)
                return false;
            if (address.Length == 0 || address.Length > 5)
                return false;

            int addressNumber;
            if (!Int32.TryParse(address, out addressNumber))
                return false;
            if (addressNumber > 0 && addressNumber <= 65536)
                return true;
            return false;
        }

        //http://msdn2.microsoft.com/en-us/library/bb508943.aspx
        public static bool ValidCell(string cell)
        {
            if (String.IsNullOrEmpty(cell))
                return false;

            if (!Regex.IsMatch(cell.Substring(0, 1), @"[A-Z]"))
                return false;

            Regex r = new Regex(@"^(?<col>\D+)(?<row>\d+)");

            string rowNumber = r.Match(cell).Result("${row}");
            string column = r.Match(cell).Result("${col}");

            return ValidRowAddress(rowNumber) && ValidColumnAddress(column);
        }

        public static object[] GetFirstRankArray(object[,] multi)
        {
            int length = multi.GetUpperBound(0) + 1;
            List<object> list = new List<object>(multi.GetUpperBound(0));
            for (int i = 1; i < length; i++)
            {
                list.Add(multi[i, 1]);
            }
            return list.ToArray();
        }

        public static object[][] ConvertMultiToJagged(object[,] multiArray)
        {
            if (multiArray == null)
            {
                return null;
            }

            object[][] jagged = new object[multiArray.GetUpperBound(0) + 1][];

            for (int i = multiArray.GetLowerBound(0); i <= multiArray.GetUpperBound(0); i++)
            {
                jagged[i] = new object[multiArray.GetUpperBound(1) + 1];

                for (int j = multiArray.GetLowerBound(1); j <= multiArray.GetUpperBound(1); j++)
                {
                    jagged[i][j] = multiArray[i, j];
                }
            }

            return jagged;
        }

        public static string[] RangeToCells(string range)
        {
            string cleanRange = RemoveWhiteSpace(range);

            return cleanRange.Split(new char[] { ':' });
        }

        public static string CellsToRange(string[] cells)
        {
            if (cells == null)
            {
                return String.Empty;
            }

            if (cells.Length > 2)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(RemoveWhiteSpace(cells[0]));
            sb.Append(":");
            sb.Append(RemoveWhiteSpace(cells[1]));
            return sb.ToString();
        }

        public static string RemoveWhiteSpace(string input)
        {
            Regex r = new Regex(@"\s");
            return r.Replace(input, String.Empty);
        }


        #region Increment Column Methods
        public static string NextColumn(string currentColumn)
        {
            char[] c = currentColumn.ToCharArray(ReplaceIndex(currentColumn), 1);
            StringBuilder sb = new StringBuilder(currentColumn);
            if (NextChar(c[0]) == 'a' || NextChar(c[0]) == 'A')
            {
                if (CountLetters(currentColumn) == 1)
                {
                    sb.Insert(0,NextChar(c[0])); ;
                }
                else
                {
                    char[] c1 = currentColumn.ToCharArray(0, 1);
                    sb.Replace(c1[0], NextChar(c1[0]), 0, 1);
                }
            }
            sb.Replace(c[0], NextChar(c[0]), ReplaceIndex(sb.ToString()), 1);
            return sb.ToString();
        }

        private static int ReplaceIndex(string s)
        {
            return CountLetters(s) - 1;
        }

        private static char NextChar(char c)
        {
            if (c == 'z')
            {
                return 'a';
            }
            if (c == 'Z')
            {
                return 'A';
            }
            return (char)((int)c + 1);
        }

        private static int CountLetters(string s)
        {
            int numLetters = 0;
            foreach (char c in s.ToCharArray())
            {
                if (Char.IsLetter(c))
                {
                    numLetters++;
                }
            }
            return numLetters;
        }
        #endregion

        public static string NextRow(string currentRow)
        {
            return currentRow.Replace(currentRow.Substring(CountLetters(currentRow)), NextNumber(Numbers(currentRow)));
        }

        private static string Numbers(string row)
        {
            return row.Substring(CountLetters(row));
        }

        private static string NextNumber(string number)
        {
            int current;
            if (Int32.TryParse(number, out current))
            {
                return Convert.ToString(++current, Format);
            }
            else
            {
                return number;
            }
        }

        public static NumberFormatInfo Format
        {
            get { return new CultureInfo("en-US", false).NumberFormat; }
        }
    }
}
