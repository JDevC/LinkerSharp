using System.Collections.Generic;
using LinkerSharp.Common.Models;
using LinkerSharp.Common.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkerSharpTests.Routing
{
    [TestClass]
    public class RouteDefinitionTest
    {
        private const string TEST_MESSAGE_NAME = "TestFoo.txt";
        private const int TEST_TRANSACT_ID = 1;

        private TransmissionMessageDTO TestMessage;
        private TransactionDTO TestTransaction;
        private RouteDefinition TestRoute;

        [TestInitialize]
        public void Init()
        {
            this.TestMessage = new TransmissionMessageDTO() { Name = TEST_MESSAGE_NAME, Content = "Test foo message." };
            this.TestTransaction = new TransactionDTO()
            {
                TransactionID = TEST_TRANSACT_ID,
                Transport = TransportTypeEnum.IN_OUT,
                RequestMessage = this.TestMessage
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
        public void TestTo()
        {
            // Execution

        }

        [TestMethod]
        public void TestSetBody()
        {
            // Execution

        }
    }
}
