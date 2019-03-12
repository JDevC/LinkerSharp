using LinkerSharp.Common.EndpointClasses;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LinkerSharpTests.EndpointClasses.Consumers
{
    [TestClass]
    public class FTPConsumerTest
    {
        private string TestFilePath;
        private EndpointFactory<IConsumer> TestConsumerFactory;
        private IConsumer TestFtpConsumer;

        [TestInitialize]
        public void Init()
        {
            this.TestFilePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "TestFiles")}\\Origin\\";

            this.TestConsumerFactory = new EndpointFactory<IConsumer>();
            this.TestFtpConsumer = this.TestConsumerFactory.GetFrom($"ftp->{this.TestFilePath}");
        }

        [TestMethod]
        public void TestReceiveMessages()
        {
            // Arrange
            var TestFileConsumer2 = this.TestConsumerFactory.GetFrom($"file->{this.TestFilePath}->foo=bar&foo2=baz");

            // Execute
            var TestTransactions = this.TestFtpConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FTP", this.TestFtpConsumer.Protocol, "FTPConsumer should have been set as 'FTP' Protocol!");
            Assert.IsTrue(this.TestFtpConsumer.Params.ContainsKey("protocol") && this.TestFtpConsumer.Params["protocol"]
                .Equals("ftp"), "FTPConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(this.TestFtpConsumer.Success, "FTPConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FTPConsumer should have receive transactions!");
            Assert.IsTrue(!TestTransactions.Any(x => x.RequestMessage == null), "FTPConsumer should have request messages!");
            Assert.IsTrue(!TestTransactions.Any(x => x.ResponseMessage == null), "FTPConsumer should have response messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(this.TestFilePath + x.RequestMessage.Name)));
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(x.ResponseMessage.Origin)));

            Assert.IsTrue(TestFileConsumer2.Params.Count == 3);
            Assert.AreEqual("bar", TestFileConsumer2.Params["foo"], "TestFileConsumer2 should have a param like 'foo=bar'!");
            Assert.AreEqual("baz", TestFileConsumer2.Params["foo2"], "TestFileConsumer2 should have a param like 'foo=bar'!");
        }

        [TestMethod]
        public void TestReceiveMessagesInJustInMode()
        {
            // Arrange
            this.TestFtpConsumer.Params.Add("just-in", "true");

            // Execute
            var TestTransactions = this.TestFtpConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FTP", this.TestFtpConsumer.Protocol, "FTPConsumer should have been set as 'FTP' Protocol!");
            Assert.IsTrue(this.TestFtpConsumer.Params.ContainsKey("protocol") && this.TestFtpConsumer.Params["protocol"].Equals("ftp"),
                "FTPConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(this.TestFtpConsumer.Success, "FTPConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FTPConsumer should have receive transactions!");
            Assert.AreEqual(TestTransactions.Count(x => x.RequestMessage == null), 0, "FTPConsumer should have request messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.ResponseMessage == null), "FTPConsumer shouldn't have response messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(this.TestFilePath + x.RequestMessage.Name)));
        }
    }
}
