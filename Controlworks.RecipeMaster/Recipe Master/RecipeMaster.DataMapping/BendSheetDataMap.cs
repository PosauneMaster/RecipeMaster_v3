using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeMaster.DataMapping
{
    public static class BendSheetDataMap
    {
        public static readonly string CUSTOMER = "D1";
        public static readonly string PART_NUMBER = "Q1";
        public static readonly string CUSTOMER_PART_NUMBER = "D3";
        public static readonly string CUSTOMER_REVISION_LEVEL = "D5";
        public static readonly string NOTES = "C7";
        public static readonly string CABLE_PN = "I1";
        public static readonly string CABLE_DESCRIPTION = "I3";
        public static readonly string AS_OF_DATE = "H5";
        public static readonly string FIRST_END = "N5";
        public static readonly string SECOND_END = "N7";
        public static readonly string FIRST_STRIP = "Q5";
        public static readonly string SECOND_STRIP = "Q7";
        public static readonly string BEND_RADIUS_CL = "D38";
        public static readonly string FIRST_END_CONNECTOR = "D40";
        public static readonly string SECOND_END_CONNECTOR = "D42";
        public static readonly string CABLE_DIAMETER = "D44";
        public static readonly string NUMBER_OF_COORDINATES = "D46";
        public static readonly string CONVERSION_FACTOR = "D48";
        public static readonly string[] INPUT_DATA = new string[] { "B11", "D35" };
        public static readonly string[] DRAWING_COORDINATES = new string[] { "G11", "I35" };
        public static readonly string[] AUTO_BEND_SHEET = new string[] { "K11", "R60" };
    }
}
