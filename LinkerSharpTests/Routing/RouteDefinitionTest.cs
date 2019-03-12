using System;
using System.Collections.Generic;
using System.IO;
using LinkerSharp.Common.Models;
using LinkerSharp.Common.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkerSharpTests.Routing
{
    [TestClass]
    public class RouteDefinitionTest
    {
        private const int TEST_TRANSACT_ID = 1;
        private const string TEST_MESSAGE_NAME = "TestFoo.txt";
        private const string TEST_MESSAGE_CONTENT = "Test foo message.";

        private TransmissionMessageDTO TestMessage;
        private TransactionDTO TestTransaction;
        private RouteDefinition TestRoute;

        [TestInitialize]
        public void Init()
        {
            this.TestMessage = new TransmissionMessageDTO() { Name = TEST_MESSAGE_NAME, Content = TEST_MESSAGE_CONTENT };
            this.TestTransaction = new TransactionDTO()
            {
                TransactionID = TEST_TRANSACT_ID,
                Transport = TransportTypeEnum.IN_OUT,
                RequestMessage = this.TestMessage,
                ResponseMessage = this.TestMessage
            };

            this.TestRoute = new RouteDefinition(new List<TransactionDTO>() { TestTransaction });
        }

        [TestMethod]
        public void TestProcess()
        {
            // Arrange
            var TestFinalTransactID = 1000;
            var TestFinalMessageName = "TestBar.md";

            // Execution
            var TestResult = this.TestRoute.Process(x => x.TransactionID = TestFinalTransactID);
            TestResult = TestResult.Process(x => x.ResponseMessage.Name = TestFinalMessageName);

            // Assertions
            Assert.IsInstanceOfType(TestResult, typeof(RouteDefinition));
            Assert.AreEqual(TestFinalTransactID, this.TestTransaction.TransactionID, $"Transaction ID should have change from {TEST_TRANSACT_ID} to {TestFinalTransactID}!");
            Assert.AreEqual(TestFinalMessageName, this.TestMessage.Name, $"Name should have change from {TEST_MESSAGE_NAME} to {TestFinalMessageName}!");
        }

        [TestMethod]
        public void TestSetHeader()
        {
            // Arrange
            var TestNewHeaderKey = "foo";
            var TestNewHeaderValue = "bar";

            // Execution
            var TestResult = this.TestRoute.SetHeader(TestNewHeaderKey, TestNewHeaderValue);

            // Assertions
            Assert.IsInstanceOfType(TestResult, typeof(RouteDefinition));
            Assert.IsTrue(TestTransaction.Headers.ContainsKey(TestNewHeaderKey), $"Transaction should have the key {TestNewHeaderKey}!");
            Assert.IsTrue(TestTransaction.Headers.ContainsValue(TestNewHeaderValue), $"Transaction should have the value {TestNewHeaderValue}!");
        }

        [TestMethod]
        public void TestSetBody()
        {
            // Arrange
            var TestMessageNewContent = "fee";
            var TestSecondMessage = new TransmissionMessageDTO() { Name = this.TestMessage.Name, Content = this.TestMessage.Content };
            var TestSecondTransaction = new TransactionDTO()
            {
                TransactionID = 1000,
                Transport = TransportTypeEnum.IN_OUT,
                RequestMessage = TestSecondMessage,
                ResponseMessage = TestSecondMessage
            };
            var TestSecondRoute = new RouteDefinition(new List<TransactionDTO>() { TestSecondTransaction });

            // Execution
            var TestResult = this.TestRoute.SetBody(TestMessageNewContent);
            var TestSecondResult = TestSecondRoute.SetBody(TestMessageNewContent, true);

            // Assertions
            Assert.IsInstanceOfType(TestResult, typeof(RouteDefinition));
            Assert.IsInstanceOfType(TestSecondResult, typeof(RouteDefinition));
            Assert.AreEqual(TestMessageNewContent, this.TestMessage.Content, $"Content should have change from {TEST_MESSAGE_CONTENT} to {TestMessageNewContent}!");
            Assert.AreEqual(TEST_MESSAGE_CONTENT + TestMessageNewContent, TestSecondMessage.Content, $"Content should have change from {TEST_MESSAGE_CONTENT} to {TEST_MESSAGE_CONTENT + TestMessageNewContent}!");
        }

        [TestMethod]
        public void TestEnrich()
        {
            // Arrange
            var TestFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles\\Enrichment");

            // Execution
            var TestResult = this.TestRoute.Enrich($"file->{TestFilePath}");

            // Assertions
            Assert.IsInstanceOfType(TestResult, typeof(RouteDefinition));
            Assert.AreNotEqual(this.TestTransaction.RequestMessage, this.TestTransaction.ResponseMessage);
        }

        [TestMethod]
        public void TestTo()
        {
            // Arrange
            var TestFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles\\Destiny");
            var TestCompleteFilePath = Path.Combine(TestFilePath, TEST_MESSAGE_NAME);

            // Execution
            this.TestRoute.To($"file->{TestFilePath}->autoclean=false");

            // Assertions
            var FileExists = File.Exists(TestCompleteFilePath);

            Assert.IsTrue(FileExists, $"A file with name {TEST_MESSAGE_NAME} should've been created at {TestFilePath}!");

            // Cleanup
            if (FileExists)
            {
                File.Delete(TestCompleteFilePath);
            }
        }
    }
}
