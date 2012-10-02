using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for OptionBoolTest and is intended
    ///to contain all OptionBoolTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OptionBoolTest
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
            OptionBool target = new OptionBool("test", false);
            target.setValueFromString("true");
            Assert.AreEqual(true, target.Value);
            target.setValueFromString("0");
            Assert.AreEqual(false, target.Value);
            target.setValueFromString("TruE");
            Assert.AreEqual(true, target.Value);
        }


        /// <summary>
        ///A test for getValueAsString
        ///</summary>
        [TestMethod()]
        public void getValueAsStringTest()
        {
            OptionBool target = new OptionBool("test", false);
            Assert.AreEqual("false", target.getValueAsString());
            target.Value = true;
            Assert.AreEqual("true", target.getValueAsString());
        }
    }
}
