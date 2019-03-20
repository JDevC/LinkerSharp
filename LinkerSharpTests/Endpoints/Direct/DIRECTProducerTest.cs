using LinkerSharp.Common;
using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkerSharpTests.Endpoints.Direct
{
    [TestClass]
    public class DIRECTProducerTest
    {
        [TestMethod]
        public void TestSendMessage()
        {
            // Arrange
            var TestContext = new LinkerSharpContext();
            var TestQueueName = "foo";
            var TestMessage = new TransmissionMessageDTO() { Content = "this is a direct test message.", Name = "", Destiny = TestQueueName };
            var TestTransaction = new TransactionDTO() { RequestMessage = TestMessage, ResponseMessage = TestMessage };

            var TestFactory = new EndpointFactory<IProducer>();
            var TestProducer = TestFactory.GetFrom($"direct->{TestQueueName}", TestContext);

            TestProducer.Transaction = TestTransaction;

            // Execution
            var Result = TestProducer.SendMessage();

            // Assertions
        }
    }
}
