using LinkerSharp.Common.Endpoints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LinkerSharpTests.Endpoints
{
    [TestClass]
    public class EndpointToolsTest
    {
        [TestMethod]
        public void TestResolveEndpoint()
        {
            //
            var TestExitProtocol = "FOO";
            var TestExitUri = "bar";

            var TestEndpointNoParams = $"{TestExitProtocol}->{TestExitUri}";
            var TestEndpointOneParam = $"{TestExitProtocol}->{TestExitUri}->param1=ban";
            var TestEndpointMultiParam = $"{TestExitProtocol}->{TestExitUri}->param1=ban&param2=baz";
            var TestEndpointError = $"{TestExitProtocol}:{TestExitUri}";

            // Execution
            EndpointTools.ResolveEndpoint(TestEndpointNoParams, out string TestProtocolNoParams, out string TestUriNoParams, out Dictionary<string, object> TestNoParams);
            EndpointTools.ResolveEndpoint(TestEndpointOneParam, out string TestProtocolOneParam, out string TestUriOneParam, out Dictionary<string, object> TestOneParam);
            EndpointTools.ResolveEndpoint(TestEndpointMultiParam, out string TestProtocolMultiParam, out string TestUriMultiParam, out Dictionary<string, object> TestMultipleParam);
            EndpointTools.ResolveEndpoint(TestEndpointError, out string TestProtocolError, out string TestUriError, out Dictionary<string, object> TestErrorParams);

            // Assertions
            Assert.AreEqual(TestExitProtocol, TestProtocolNoParams);
            Assert.AreEqual(TestExitUri, TestUriNoParams);
            Assert.AreEqual(1, TestNoParams.Count);
            Assert.AreEqual(TestExitProtocol.ToLower(), TestNoParams["protocol"]);

            Assert.AreEqual(TestExitProtocol, TestProtocolOneParam);
            Assert.AreEqual(TestExitUri, TestUriOneParam);
            Assert.AreEqual(2, TestOneParam.Count);
            Assert.AreEqual(TestExitProtocol.ToLower(), TestOneParam["protocol"]);
            Assert.AreEqual("ban", TestOneParam["param1"]);

            Assert.AreEqual(TestExitProtocol, TestProtocolMultiParam);
            Assert.AreEqual(TestExitUri, TestUriMultiParam);
            Assert.AreEqual(3, TestMultipleParam.Count);
            Assert.AreEqual(TestExitProtocol.ToLower(), TestMultipleParam["protocol"]);
            Assert.AreEqual("ban", TestMultipleParam["param1"]);
            Assert.AreEqual("baz", TestMultipleParam["param2"]);

            Assert.AreEqual("", TestProtocolError);
            Assert.AreEqual("", TestUriError);
            Assert.AreEqual(0, TestErrorParams.Count);
        }
    }
}
