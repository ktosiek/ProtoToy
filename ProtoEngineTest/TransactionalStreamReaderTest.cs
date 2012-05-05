using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for TransactionalStreamReaderTest and is intended
    ///to contain all TransactionalStreamReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransactionalStreamReaderTest
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
        ///A test for cancelTransaction
        ///</summary>
        [TestMethod()]
        public void cancelTransactionTest()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            TransactionalStreamReader target = new TransactionalStreamReader(stream); // TODO: Initialize to an appropriate value
            target.startTransaction();
            target.cancelTransaction();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for commitTransaction
        ///</summary>
        [TestMethod()]
        public void commitTransactionTest()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            TransactionalStreamReader target = new TransactionalStreamReader(stream); // TODO: Initialize to an appropriate value
            target.startTransaction();
            target.commitTransaction();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for startTransaction
        ///</summary>
        [TestMethod()]
        public void startTransactionTest()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            TransactionalStreamReader target = new TransactionalStreamReader(stream); // TODO: Initialize to an appropriate value
            target.startTransaction();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
