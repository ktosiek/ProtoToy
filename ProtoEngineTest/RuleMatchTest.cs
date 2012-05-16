using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using System.Collections.Generic;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for RuleMatchTest and is intended
    ///to contain all RuleMatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RuleMatchTest
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
        [DeploymentItem("ProtoEngine.dll")]
        public void matchTest()
        {
            XmlDocument node = new XmlDocument();
            node.LoadXml("<match name=\"test\" value=\"10\"/>");
            RuleMatch rule = new RuleMatch(node.FirstChild, null);
            Dictionary<String, Option> env = new Dictionary<string, Option>();
            env.Add("test", new OptionInt("test", 10, 0, 255, 1));
            Assert.IsNotNull(rule.match(env, null));
            node.LoadXml("<match name=\"test\" value=\"0\"/>");
            rule = new RuleMatch(node.FirstChild, null);
            Assert.IsNull(rule.match(env, null));
        }
    }
}
