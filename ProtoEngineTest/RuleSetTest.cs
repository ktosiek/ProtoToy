using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using System.Collections.Generic;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for RuleSetTest and is intended
    ///to contain all RuleSetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RuleSetTest
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
        ///A test for match
        ///</summary>
        [TestMethod()]
        public void matchTest()
        {
            XmlDocument node = new XmlDocument();
            node.LoadXml("<set name=\"test\" value=\"10\"/>");
            Rule rule = new RuleSet(node.FirstChild, null);
            Dictionary<String, Option> env = new Dictionary<string, Option>();
            env = rule.match(env, null);
            Assert.AreEqual(new OptionInt("", 10, 0, 255, 1), env["test"]);
        }
    }
}
