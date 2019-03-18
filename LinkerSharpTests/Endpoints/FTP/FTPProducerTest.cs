using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.FTP;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace LinkerSharpTests.Endpoints.FTP
{
    [TestClass]
    public class FTPProducerTest
    {
        private string TestFilePath;
        private IProducer TestProducer;
        private TransmissionMessageDTO TestMessage;

        [TestInitialize]
        public void Init()
        {
            this.TestFilePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles")}\\Destiny\\";

            var TestFactory = new EndpointFactory<IProducer>();
            this.TestProducer = new FTPProducer(this.TestFilePath, new FTPConnectorMock(FTPConnectorMock.Behaviour.SUCCESS));
            
            this.TestMessage = new TransmissionMessageDTO() { Content = "This is a test file.", Name = "Testfile.txt", Destiny = this.TestFilePath };
        }

        [TestMethod]
        public void TestSendMessage()
        {
            // Arrange
            var TestTransaction = new TransactionDTO()
            {
                RequestMessage = TestMessage,
                ResponseMessage = TestMessage,
                Headers = new Dictionary<string, string>() { { "autoclean", "false" } }
            };

            TestProducer.Transaction = TestTransaction;

            // Execute
            var TestMessages = TestProducer.SendMessage();

            // Assertions
            Assert.AreEqual("FILE", TestProducer.Protocol, "FILEProducer should have been set as 'FILE' Protocol!");
            Assert.IsTrue(TestProducer.Success, "FILEProducer should have been successfull!");
        }

        [TestCleanup]
        public void CleanFiles()
        {
            foreach (var TestProducedFile in Directory.GetFiles(this.TestFilePath))
            {
                System.IO.File.Delete(TestProducedFile);
            }
        }
    }
}
