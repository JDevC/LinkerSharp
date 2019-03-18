using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LinkerSharpTests.Endpoints.File
{
    [TestClass]
    public class FILEConsumerTest
    {
        private string TestFilePath;
        private EndpointFactory<IConsumer> TestConsumerFactory;
        private IConsumer TestFileConsumer;

        [TestInitialize]
        public void Init()
        {
            this.TestFilePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "TestFiles")}\\Origin\\";

            this.TestConsumerFactory = new EndpointFactory<IConsumer>();
            this.TestFileConsumer = this.TestConsumerFactory.GetFrom($"file->{this.TestFilePath}");
        }

        [TestMethod]
        public void TestReceiveMessages()
        {
            // Arrange
            var TestFileConsumer2 = this.TestConsumerFactory.GetFrom($"file->{this.TestFilePath}->foo=bar&foo2=baz");

            // Execute
            var TestTransactions = this.TestFileConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FILE", this.TestFileConsumer.Protocol, "FILEConsumer should have been set as 'FILE' Protocol!");
            Assert.IsTrue(this.TestFileConsumer.Params.ContainsKey("protocol") && this.TestFileConsumer.Params["protocol"]
                .Equals("file"), "FILEConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(this.TestFileConsumer.Success, "FILEConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FILEConsumer should have receive transactions!");
            Assert.IsTrue(!TestTransactions.Any(x => x.RequestMessage == null), "FILEConsumer should have request messages!");
            Assert.IsTrue(!TestTransactions.Any(x => x.ResponseMessage == null), "FILEConsumer should have response messages!");
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
            this.TestFileConsumer.Params.Add("just-in", "true");

            // Execute
            var TestTransactions = this.TestFileConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FILE", this.TestFileConsumer.Protocol, "FILEConsumer should have been set as 'FILE' Protocol!");
            Assert.IsTrue(this.TestFileConsumer.Params.ContainsKey("protocol") && this.TestFileConsumer.Params["protocol"].Equals("file"),
                "FILEConsumer should have the param {'protocol' = 'file'}!");
            Assert.IsTrue(this.TestFileConsumer.Success, "FILEConsumer should have been successfull!");
            Assert.AreNotEqual(TestTransactions.Count, 0, "FILEConsumer should have receive transactions!");
            Assert.AreEqual(TestTransactions.Count(x => x.RequestMessage == null), 0, "FILEConsumer should have request messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.ResponseMessage == null), "FILEConsumer shouldn't have response messages!");
            Assert.IsTrue(TestTransactions.Any(x => x.RequestMessage.Origin.Equals(this.TestFilePath + x.RequestMessage.Name)));
        }
    }
}
