using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for OptionIntTest and is intended
    ///to contain all OptionIntTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OptionIntTest
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
            OptionInt target = new OptionInt("test", 0, 0, 255, 1); // Jednobajtowe 0
            target.setValueFromString("10");
            Assert.AreEqual(10, target.Value);
        }
    }
}
