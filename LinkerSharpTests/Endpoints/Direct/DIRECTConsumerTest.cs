using LinkerSharp.Common;
using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LinkerSharpTests.Endpoints.Direct
{
    [TestClass]
    public class DIRECTConsumerTest
    {
        [TestMethod]
        public void TestSendMessage()
        {
            // Arrange
            var TestQueueName = "foo";
            var TestTransactionID = 100;
            var TestHeaders = new Dictionary<string, object> { { "bar", "baz" } };
            var TestRequestMessage = new TransmissionMessageDTO() { Content = "Test message!" };
            var TestTransaction = new TransactionDTO() { TransactionID = TestTransactionID, Headers = TestHeaders, RequestMessage = TestRequestMessage };

            var TestContext = new LinkerSharpContext();
            TestContext.DirectQueues.Add(TestQueueName, new Queue<TransactionDTO>());
            TestContext.DirectQueues[TestQueueName].Enqueue(TestTransaction);

            var TestFactory = new EndpointFactory<IConsumer>();
            var TestConsumer = TestFactory.GetFrom($"direct->{TestQueueName}", TestContext);

            // Execution
            var TestResults = TestConsumer.ReceiveMessages();

            // Assertions
            Assert.AreEqual(1, TestResults.Count, "TestConsumer should have results!");
            Assert.AreEqual(TestTransactionID, TestResults[0].TransactionID);
            Assert.AreEqual(TestHeaders["bar"], TestResults[0].Headers["bar"]);
            Assert.AreEqual(TestRequestMessage.Content, TestResults[0].RequestMessage.Content);
            Assert.IsTrue(TestResults[0].ResponseMessage == null);
        }
    }
}
