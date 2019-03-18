using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.FTP;
using LinkerSharp.Common.Endpoints.IFaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LinkerSharpTests.Endpoints.FTP
{
    [TestClass]
    public class FTPConsumerTest
    {
        private string TestFtpUrl;
        private EndpointFactory<IConsumer> TestConsumerFactory;

        [TestInitialize]
        public void Init()
        {
            this.TestFtpUrl = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "TestFiles")}\\Origin\\";

            this.TestConsumerFactory = new EndpointFactory<IConsumer>();
            //this.TestFtpConsumer = this.TestConsumerFactory.GetFrom($"ftp->{this.TestFtpUrl}");
        }

        [TestMethod]
        public void TestReceiveMessages()
        {
            // Arrange
            var TestFtpConsumer = new FTPConsumer($"ftp->{this.TestFtpUrl}", new FTPConnectorMock(FTPConnectorMock.Behaviour.SUCCESS));
            var TestFtpConsumer2 = this.TestConsumerFactory.GetFrom($"ftp->{this.TestFtpUrl}->foo=bar&foo2=baz");

            // Execute
            var TestTransactions = TestFtpConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FTP", TestFtpConsumer.Protocol, "FTPConsumer should have been set as 'FTP' Protocol!");
            Assert.IsTrue(TestFtpConsumer.Params.ContainsKey("protocol") && TestFtpConsumer.Params["protocol"]
                .Equals("ftp"), "FTPConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(TestFtpConsumer.Success, "FTPConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FTPConsumer should have receive transactions!");
            Assert.IsTrue(!TestTransactions.Any(x => x.RequestMessage == null), "FTPConsumer should have request messages!");
            Assert.IsTrue(!TestTransactions.Any(x => x.ResponseMessage == null), "FTPConsumer should have response messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(this.TestFtpUrl + x.RequestMessage.Name)));
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(x.ResponseMessage.Origin)));

            Assert.IsTrue(TestFtpConsumer2.Params.Count == 3);
            Assert.AreEqual("bar", TestFtpConsumer2.Params["foo"], "TestFtpConsumer2 should have a param like 'foo=bar'!");
            Assert.AreEqual("baz", TestFtpConsumer2.Params["foo2"], "TestFtpConsumer2 should have a param like 'foo=bar'!");
        }

        [TestMethod]
        public void TestReceiveMessagesErrors()
        {
            // Arrange
            var TestFtpConsumer = new FTPConsumer($"ftp->{this.TestFtpUrl}", new FTPConnectorMock(FTPConnectorMock.Behaviour.NO_SUCCESS));
            var TestFtpConsumer2 = new FTPConsumer($"ftp->{this.TestFtpUrl}", new FTPConnectorMock(FTPConnectorMock.Behaviour.WEB_EXCEPTION));

            // Execute
            var TestTransactions = TestFtpConsumer.ReceiveMessages();
            var TestTransactions2 = TestFtpConsumer2.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FTP", TestFtpConsumer.Protocol, "TestFtpConsumer should have been set as 'FTP' Protocol!");
            Assert.AreEqual("FTP", TestFtpConsumer2.Protocol, "TestFtpConsumer2 should have been set as 'FTP' Protocol!");
            Assert.IsTrue(TestFtpConsumer.Params.ContainsKey("protocol") && TestFtpConsumer.Params["protocol"]
                .Equals("ftp"), "TestFtpConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(TestFtpConsumer2.Params.ContainsKey("protocol") && TestFtpConsumer2.Params["protocol"]
                .Equals("ftp"), "TestFtpConsumer2 should have the param {'protocol' = 'file'}!");
            Assert.IsFalse(TestFtpConsumer.Success, "TestFtpConsumer should haven't been successfull!");
            Assert.IsFalse(TestFtpConsumer2.Success, "TestFtpConsumer2 should haven't been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "TestFtpConsumer should have receive transactions!");
            Assert.AreNotEqual(TestTransactions2.Count, 0, "TestFtpConsumer2 should have receive transactions!");
            Assert.AreEqual("Error", TestTransactions[0].RequestMessage.Error.Code, "TestTransactions' error code should be 'Error'!");
            Assert.IsTrue(TestTransactions[0].RequestMessage.Error.Reason.Contains("Error reason from FTP Mock."));
            Assert.AreEqual("", TestTransactions2[0].RequestMessage.Error.Code, "TestTransactions2's error code should be empty!");
            Assert.IsTrue(TestTransactions2[0].RequestMessage.Error.Reason.Contains("Web exception from FTP Mock"));
        }

        [TestMethod]
        public void TestReceiveMessagesInJustInMode()
        {
            // Arrange
            var TestFtpConsumer = new FTPConsumer($"ftp->{this.TestFtpUrl}", new FTPConnectorMock(FTPConnectorMock.Behaviour.SUCCESS));
            TestFtpConsumer.Params.Add("just-in", "true");

            // Execute
            var TestTransactions = TestFtpConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FTP", TestFtpConsumer.Protocol, "FTPConsumer should have been set as 'FTP' Protocol!");
            Assert.IsTrue(TestFtpConsumer.Params.ContainsKey("protocol") && TestFtpConsumer.Params["protocol"].Equals("ftp"),
                "FTPConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(TestFtpConsumer.Success, "FTPConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FTPConsumer should have receive transactions!");
            Assert.AreEqual(TestTransactions.Count(x => x.RequestMessage == null), 0, "FTPConsumer should have request messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.ResponseMessage == null), "FTPConsumer shouldn't have response messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(this.TestFtpUrl + x.RequestMessage.Name)));
        }
    }
}
