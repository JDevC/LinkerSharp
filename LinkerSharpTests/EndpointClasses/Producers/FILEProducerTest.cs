using LinkerSharp.Common.EndpointClasses;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using LinkerSharp.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace LinkerSharpTests.EndpointClasses.Producers
{
    [TestClass]
    public class FILEProducerTest
    {
        private string TestFilePath;
        private IProducer TestProducer;
        private TransmissionMessageDTO TestMessage;

        [TestInitialize]
        public void Init()
        {
            this.TestFilePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles")}\\Destiny\\";

            var TestFactory = new EndpointFactory<IProducer>();
            this.TestProducer = TestFactory.GetFrom($"file->{this.TestFilePath}");
            
            this.TestMessage = new TransmissionMessageDTO() { Content = "this is a test file.", Name = "Testfile.txt", Destiny = this.TestFilePath };
        }

        [TestMethod]
        public void TestSendMessageNoAutoclean()
        {
            // Arrange
            var TestTransaction = new TransactionDTO()
            {
                RequestMessage = TestMessage,
                ResponseMessage = TestMessage,
                Properties = new Dictionary<string, string>() { { "autoclean", "false" } }
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
                File.Delete(TestProducedFile);
            }
        }
    }
}
