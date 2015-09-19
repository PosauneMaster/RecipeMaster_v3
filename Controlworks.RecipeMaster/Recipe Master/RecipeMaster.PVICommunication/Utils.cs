using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using RecipeMaster.Services;

namespace BendSheets.PVICommunication
{
    public static class Utils
    {

        public static int FindNumber(object data)
        {
            if (data == null)
            {
                return 0;
            }
            return FindNumber(data.ToString());
        }

        public static int FindNumber(string input)
        {
            string number = System.Text.RegularExpressions.Regex.Replace(input, @"\D", "");
            return Convert.ToInt32(number, RecipeMasterServices.Format);
        }
    }
}
