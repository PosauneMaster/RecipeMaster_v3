using System;
using BendSheets;
using NUnit.Framework;
using System.IO;

namespace ControlWorks.RecipeMaster.Tests
{
    [TestFixture]
    public class TestRecipeVariable
    {
        private RecipeTemplates m_RecipeTemplate;
        private RecipeTemplateItem item1;
        private RecipeTemplateItem item2;
        private RecipeTemplateItem item3;
        private RecipeTemplateItem item4;
        private RecipeTemplateItem item5;
        private RecipeTemplateItem item6;
        private RecipeTemplateItem item7;
        private RecipeTemplateItem item8;
        private RecipeTemplateItem item9;
        private RecipeTemplateItem item10;

        [SetUp]
        public void SetUp()
        {
            //m_RecipeTemplate = new RecipeTemplates();

            //item1 = new RecipeTemplateItem("Test1", RecipeTemplateItemType.Cell, "A1", String.Empty);
            //m_RecipeTemplate.AddTemplate(item1);

            //item2 = new RecipeTemplateItem("Test2", RecipeTemplateItemType.Cell, "A2", String.Empty);
            //m_RecipeTemplate.AddTemplate(item2);

            //item3 = new RecipeTemplateItem("Test3", RecipeTemplateItemType.Cell, "A3", String.Empty);
            //m_RecipeTemplate.AddTemplate(item3);

            //item4 = new RecipeTemplateItem("Test4", RecipeTemplateItemType.Cell, "A4", String.Empty);
            //m_RecipeTemplate.AddTemplate(item4);

            //item5 = new RecipeTemplateItem("Test5", RecipeTemplateItemType.Cell, "A5", String.Empty);
            //m_RecipeTemplate.AddTemplate(item5);

            //item6 = new RecipeTemplateItem("Test6", RecipeTemplateItemType.Range, "B1", "C3");
            //m_RecipeTemplate.AddTemplate(item6);

            //item7 = new RecipeTemplateItem("Test7", RecipeTemplateItemType.Range, "D1", "E5");
            //m_RecipeTemplate.AddTemplate(item7);

            //item8 = new RecipeTemplateItem("Test8", RecipeTemplateItemType.Range, "F1", "G10");
            //m_RecipeTemplate.AddTemplate(item8);

            //item9 = new RecipeTemplateItem("Test9", RecipeTemplateItemType.Range, "H1", "I23");
            //m_RecipeTemplate.AddTemplate(item9);

            //item10 = new RecipeTemplateItem("Test10", RecipeTemplateItemType.Range, "M1", "P10");
            //m_RecipeTemplate.AddTemplate(item10);
        }

        [Test]
        public void TestCreateRecipeVariableObjectArray()
        {
            object[,] o = new object[5,2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 }, { 9, 10 } };
            RecipeVariable rv = new RecipeVariable("ArrayTest", o);

            int upperBound0 = o.GetUpperBound(0) + 1;
            int upperBound1 = o.GetUpperBound(1) + 1;


            for (int i = 0; i < upperBound0; i++)
            {
                Assert.AreEqual(o[i, 0], rv.ValueArray[i, 0]);
                Assert.AreEqual(o[i, 1], rv.ValueArray[i, 1]);
            }
        }

        [Test]
        public void TestCreateRecipeVariableInt()
        {
            Int32 i = 1024;
            RecipeVariable rv = new RecipeVariable("Int32Test", i);

            Int32 converted = rv;
            Assert.IsTrue(converted == 1024);


        }

        [Test]
        public void TestRecipeTemplate()
        {
            Assert.IsTrue(item1.Equals(m_RecipeTemplate["Test1"]));
            Assert.IsTrue(item2.Equals(m_RecipeTemplate["Test2"]));
            Assert.IsTrue(item3.Equals(m_RecipeTemplate["Test3"]));
            Assert.IsTrue(item4.Equals(m_RecipeTemplate["Test4"]));
            Assert.IsTrue(item5.Equals(m_RecipeTemplate["Test5"]));
            Assert.IsTrue(item6.Equals(m_RecipeTemplate["Test6"]));
            Assert.IsTrue(item7.Equals(m_RecipeTemplate["Test7"]));
            Assert.IsTrue(item8.Equals(m_RecipeTemplate["Test8"]));
            Assert.IsTrue(item9.Equals(m_RecipeTemplate["Test9"]));
            Assert.IsTrue(item10.Equals(m_RecipeTemplate["Test10"]));
        }

        [Test]
        public void TestSerializeRecipeTemplate()
        {
            string path = @"C:\temp\Template1.xml";
            m_RecipeTemplate.Save(path);
            Assert.IsTrue(File.Exists(path));

            RecipeTemplates templates = new RecipeTemplates();
            templates.Load(path);

            Assert.AreEqual(m_RecipeTemplate.Count, templates.Count);

            Assert.IsTrue(item1.Equals(templates["Test1"]));
            Assert.IsTrue(item2.Equals(templates["Test2"]));
            Assert.IsTrue(item3.Equals(templates["Test3"]));
            Assert.IsTrue(item4.Equals(templates["Test4"]));
            Assert.IsTrue(item5.Equals(templates["Test5"]));
            Assert.IsTrue(item6.Equals(templates["Test6"]));
            Assert.IsTrue(item7.Equals(templates["Test7"]));
            Assert.IsTrue(item8.Equals(templates["Test8"]));
            Assert.IsTrue(item9.Equals(templates["Test9"]));
            Assert.IsTrue(item10.Equals(templates["Test10"]));
        }
    }
}
