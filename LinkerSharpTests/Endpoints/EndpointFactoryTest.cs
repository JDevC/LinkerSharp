using LinkerSharp.Common;
using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LinkerSharpTests.Endpoints
{
    [TestClass]
    public class EndpointFactoryTest
    {
        private LinkerSharpContext TestContext;

        [TestInitialize]
        public void Init()
        {
            this.TestContext = new LinkerSharpContext();
        }

        [TestMethod]
        public void TestGetConsumer()
        {
            // Arrange
            var TestFactory = new EndpointFactory<IConsumer>();
            var TestFileEndpoint = "\\foo\bar";
            var TestFTPEndpoint = "ftp://foo/bar";

            // Execute
            var TestExistingFileConsumer = TestFactory.GetFrom($"file->{TestFileEndpoint}", this.TestContext);
            var TestExistingFtpConsumer = TestFactory.GetFrom($"ftp->{TestFTPEndpoint}", this.TestContext);

            // Assertions
            Assert.AreEqual("FILE", TestExistingFileConsumer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual("FTP", TestExistingFtpConsumer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual(TestFileEndpoint, TestExistingFileConsumer.Endpoint, $"Wrong endpoint assigned!");
            Assert.AreEqual(TestFTPEndpoint, TestExistingFtpConsumer.Endpoint, $"Wrong endpoint assigned!");
        }

        [TestMethod]
        public void TestGetProducer()
        {
            // Arrange
            var TestFactory = new EndpointFactory<IProducer>();
            var TestFileEndpoint = "\\foo\bar";
            var TestFTPEndpoint = "ftp://foo/bar";

            // Execute
            var TestExistingFileProducer = TestFactory.GetFrom($"file->{TestFileEndpoint}", this.TestContext);
            var TestExistingFtpProducer = TestFactory.GetFrom($"ftp->{TestFTPEndpoint}", this.TestContext);

            // Assertions
            Assert.AreEqual("FILE", TestExistingFileProducer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual("FTP", TestExistingFtpProducer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual(TestFileEndpoint, TestExistingFileProducer.Endpoint, $"Wrong endpoint assigned!");
            Assert.AreEqual(TestFTPEndpoint, TestExistingFtpProducer.Endpoint, $"Wrong endpoint assigned!");
        }

        [TestMethod]
        public void TestBadRouteTags()
        {
            // Arrange
            var CorrectExceptionMsg = "Consumer with name {0} was not found.";
            var UnexpectedExMsg = "Unexpected exception message!";
            var UnexpectedEx = "Unexpected exception: {0}";
            var NotThrownFailMsg = "TestBadEndpoint should throw an exception!";

            var TestConsumerFactory = new EndpointFactory<IConsumer>();
            var TestProducerFactory = new EndpointFactory<IProducer>();

            // Execution and assertions
            try
            {
                var TestBadEndpoint = TestConsumerFactory.GetFrom(@"foo->\\foo\bar", this.TestContext);
                Assert.Fail(NotThrownFailMsg);
            }
            catch (KeyNotFoundException NotFoundEx)
            {
                Assert.AreEqual(string.Format(CorrectExceptionMsg, "FOOConsumer"), NotFoundEx.Message, UnexpectedExMsg);
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format(UnexpectedEx, ex.Message));
            }

            try
            {
                var TestBadEndpoint = TestProducerFactory.GetFrom(@"foo->\\foo\bar", this.TestContext);
                Assert.Fail(NotThrownFailMsg);
            }
            catch (KeyNotFoundException NotFoundEx)
            {
                Assert.AreEqual(string.Format(CorrectExceptionMsg, "FOOProducer"), NotFoundEx.Message, UnexpectedExMsg);
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format(UnexpectedEx, ex.Message));
            }
        }
    }
}
