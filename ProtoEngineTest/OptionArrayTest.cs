using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for OptionArrayTest and is intended
    ///to contain all OptionArrayTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OptionArrayTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for setValueFromString
        ///</summary>
        [TestMethod()]
        public void setValueFromStringTest()
        {
            OptionArray target = new OptionArray("test", "array int 3");
            target.setValueFromString(" 1 ,2 ,3 ");
            Assert.AreEqual(1, ((OptionInt)target.getOption(0)).Value);
            Assert.AreEqual(2, ((OptionInt)target.getOption(1)).Value);
            Assert.AreEqual(3, ((OptionInt)target.getOption(2)).Value);
        }
    }
}
