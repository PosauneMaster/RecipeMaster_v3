using System;

namespace ControlWorks.RecipeMaster
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
