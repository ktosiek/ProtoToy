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
            Stream stream = new MemoryStream();
            TransactionalStreamReader target = new TransactionalStreamReader(stream);
            // Przygotuj dane testowe
            stream.WriteByte(9);
            stream.WriteByte(20);
            stream.WriteByte(31);
            stream.WriteByte(42);
            stream.Seek(0, SeekOrigin.Begin);
            target.startTransaction();
            Assert.AreEqual(target.ReadByte(), 9);
            Assert.AreEqual(target.ReadByte(), 20);
            target.cancelTransaction();
            target.startTransaction();
            Assert.AreEqual(target.ReadByte(), 9);
            Assert.AreEqual(target.ReadByte(), 20);
            Assert.AreEqual(target.ReadByte(), 31);
            Assert.AreEqual(target.ReadByte(), 42);
            target.cancelTransaction();
            Assert.AreEqual(stream.ReadByte(), -1); // Cały strumien został zjedzony
        }

        /// <summary>
        ///A test for commitTransaction
        ///</summary>
        [TestMethod()]
        public void commitTransactionTest()
        {
            Stream stream = new MemoryStream();
            TransactionalStreamReader target = new TransactionalStreamReader(stream);
            // Przygotuj dane testowe
            stream.WriteByte(9);
            stream.WriteByte(20);
            stream.WriteByte(31);
            stream.WriteByte(42);
            stream.Seek(0, SeekOrigin.Begin);
            target.startTransaction();
            Assert.AreEqual(target.ReadByte(), 9);
            Assert.AreEqual(target.ReadByte(), 20);
            target.commitTransaction();
            target.startTransaction();
            Assert.AreEqual(target.ReadByte(), 31);
            Assert.AreEqual(target.ReadByte(), 42);
            target.commitTransaction();
            Assert.AreEqual(stream.ReadByte(), -1); // Cały strumien został zjedzony
        }
    }
}
