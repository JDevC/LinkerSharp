using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LinkerSharpTests.Endpoints
{
    [TestClass]
    public class EndToEndTest
    {
        [TestMethod]
        public void TestFullRouteEndpoints()
        {
            // Arrange
            var TestFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles");
            var TestDestiny = $"{TestFilePath}\\Destiny\\";

            var TestConsumerFactory = new EndpointFactory<IConsumer>();
            var TestProducerFactory = new EndpointFactory<IProducer>();

            var TestFileConsumer = TestConsumerFactory.GetFrom($"file->{TestFilePath}\\Origin\\->autoclean=false");
            var TestFileProducer = TestProducerFactory.GetFrom($"file->{TestDestiny}");

            // Execute
            var TestTransactions = TestFileConsumer.ReceiveMessages();
            var TestResults = new List<bool>();

            foreach (var TestTransaction in TestTransactions)
            {
                var Date = DateTime.Now;

                TestFileProducer.Transaction = TestTransaction;
                TestFileProducer.Transaction.ResponseMessage.Content = TestTransaction.RequestMessage.Content;
                TestFileProducer.Transaction.ResponseMessage.Name = $"TestReal_{Date.Year}_{Date.Month}_{Date.Day}_{Date.Hour}_{Date.Minute}_{Date.Millisecond}.txt";

                TestResults.Add(TestFileProducer.SendMessage());
            }

            this.CleanFiles(TestDestiny);

            // Assertions
            Assert.AreEqual(TestTransactions.Count(), TestResults.Count(t => t), "There are unsent messages!");
        }

        private void CleanFiles(string DestinyPath)
        {
            foreach (var TestFile in Directory.GetFiles(DestinyPath))
            {
                System.IO.File.Delete(TestFile);
            }
        }
    }
}
