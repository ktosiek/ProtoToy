using ProtoEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace ProtoEngineTest
{
    
    
    /// <summary>
    ///This is a test class for ProtocolTest and is intended
    ///to contain all ProtocolTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProtocolTest
    {
        private Protocol protocol;

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
        ///A test for Protocol Constructor
        ///</summary>
        [TestInitialize()]
        public void ProtocolConstructorTest()
        {
            protocol = new Protocol("..\\..\\..\\ProtoEngine\\Modbus-serial.xml");
        }

        /// <summary>
        ///A test for DevicePrototypes
        ///</summary>
        [TestMethod()]
        public void DevicePrototypesTest()
        {
            Assert.IsTrue(protocol.DevicePrototypes.Count > 0);
        }

        /// <summary>
        ///A test for Options
        ///</summary>
        [TestMethod()]
        public void OptionsTest()
        {
            Assert.IsTrue(protocol.Options.Count > 0);
        }

        /// <summary>
        ///A test for RegisteredDevices
        ///</summary>
        [TestMethod()]
        public void RegisteredDevicesTest()
        {
            Assert.IsTrue(protocol.RegisteredDevices.Count == 0);
        }

        /// <summary>
        ///A test for registerDevice
        ///</summary>
        [TestMethod()]
        public void registerDeviceTest()
        {
            protocol.registerDevice(protocol.DevicePrototypes[0].create());
            Assert.IsTrue(protocol.RegisteredDevices.Count == 1);
        }

        /// <summary>
        ///A test for unregisterDevice
        ///</summary>
        [TestMethod()]
        public void unregisterDeviceTest()
        {
            Device device = protocol.DevicePrototypes[0].create();
            protocol.registerDevice(device);
            protocol.unregisterDevice(device);
            protocol.unregisterDevice(protocol.DevicePrototypes[0].create());
            Assert.IsTrue(protocol.RegisteredDevices.Count == 0);
        }
    }
}
