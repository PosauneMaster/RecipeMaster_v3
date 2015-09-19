using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WH.ComUtils.ExcelManager;

namespace ControlWorks.RecipeMaster.Tests
{
    [TestFixture]
    public class TestRecipeData
    {
        [Test]
        public void TestMultiToSingle()
        {
            object[,] multi = new object[5, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 }, { 9, 10 } };
            object[] oArray = ExcelUtils.GetFirstRankArray(multi);

            Assert.AreEqual(5, oArray.Length);
            Assert.AreEqual(1, oArray[0]);
            Assert.AreEqual(3, oArray[1]);
            Assert.AreEqual(5, oArray[2]);
            Assert.AreEqual(7, oArray[3]);
            Assert.AreEqual(9, oArray[4]);
        }

        [Test]
        public void TestMapping()
        {
            string s = "first.part[]last.part";
            string[] split = s.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries );
        }
    }
}
