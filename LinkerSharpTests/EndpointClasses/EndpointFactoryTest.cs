using LinkerSharp.Common.EndpointClasses;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LinkerSharp.EndpointClasses
{
    [TestClass]
    public class EndpointFactoryTest
    {
        [TestMethod]
        public void TestGetSeseConsumer()
        {
            // Arrange
            var TestFactory = new EndpointFactory<IConsumer>();
            var TestFileEndpoint = "\\foo\bar";
            var TestFTPEndpoint = "ftp://foo/bar";

            // Execute
            var TestExistingFileConsumer = TestFactory.GetFrom($"file->{TestFileEndpoint}");
            var TestExistingFtpConsumer = TestFactory.GetFrom($"ftp->{TestFTPEndpoint}");

            // Assertions
            Assert.AreEqual("FILE", TestExistingFileConsumer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual("FTP", TestExistingFtpConsumer.Protocol, $"Wrong protocol assigned!");
            Assert.AreEqual(TestFileEndpoint, TestExistingFileConsumer.Endpoint, $"Wrong endpoint assigned!");
            Assert.AreEqual(TestFTPEndpoint, TestExistingFtpConsumer.Endpoint, $"Wrong endpoint assigned!");
        }

        [TestMethod]
        public void TestGetSeseProducer()
        {
            // Arrange
            var TestFactory = new EndpointFactory<IProducer>();
            var TestFileEndpoint = "\\foo\bar";
            var TestFTPEndpoint = "ftp://foo/bar";

            // Execute
            var TestExistingFileProducer = TestFactory.GetFrom($"file->{TestFileEndpoint}");
            var TestExistingFtpProducer = TestFactory.GetFrom($"ftp->{TestFTPEndpoint}");

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
                var TestBadEndpoint = TestConsumerFactory.GetFrom(@"foo->\\foo\bar");
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
                var TestBadEndpoint = TestProducerFactory.GetFrom(@"foo->\\foo\bar");
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
