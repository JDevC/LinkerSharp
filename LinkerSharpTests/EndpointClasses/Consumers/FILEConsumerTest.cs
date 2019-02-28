using LinkerSharp.Common.EndpointClasses;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinkerSharpTests.EndpointClasses.Consumers
{
    [TestClass]
    public class FILEConsumerTest
    {
        private string TestFilePath;

        [TestInitialize]
        public void Init()
        {
            this.TestFilePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "TestFiles")}\\Origin\\";
        }

        [TestMethod]
        public void TestReceiveMessages()
        {
            // Arrange
            var TestConsumerFactory = new EndpointFactory<IConsumer>();

            var TestFileConsumer = TestConsumerFactory.GetFrom($"file->{this.TestFilePath}");
            var TestFileConsumer2 = TestConsumerFactory.GetFrom($"file->{this.TestFilePath}->foo=bar&foo2=baz");

            // Execute
            var TestMessages = TestFileConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual("FILE", TestFileConsumer.Protocol, "FILEConsumer should have been set as 'FILE' Protocol!");
            Assert.IsTrue(TestFileConsumer.Params.Count == 1);
            Assert.IsTrue(TestFileConsumer.Success, "FILEConsumer should have been successfull!");
            Assert.IsTrue(TestMessages.Count > 0, "FILEConsumer should have receive messages!");

            Assert.IsTrue(TestFileConsumer2.Params.Count == 3);
            Assert.AreEqual("bar", TestFileConsumer2.Params["foo"], "TestFileConsumer2 should have a param like 'foo=bar'!");
            Assert.AreEqual("baz", TestFileConsumer2.Params["foo2"], "TestFileConsumer2 should have a param like 'foo=bar'!");
        }
    }
}
