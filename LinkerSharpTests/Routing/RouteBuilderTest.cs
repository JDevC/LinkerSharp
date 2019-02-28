using System;
using LinkerSharp.Common.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkerSharpTests.Routing
{
    [TestClass]
    public class RouteBuilderTest
    {
        [TestMethod]
        public void TestFrom()
        {
            // Arrange
            var TestFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "TestFiles");

            var TestRouteBuilder = new DummyRouteBuilder();

            // Execution
            var TestResult = TestRouteBuilder.From($"file->{TestFilePath}\\Origin\\");

            // Assertions
            Assert.IsInstanceOfType(TestResult, typeof(RouteDefinition));
        }
    }
}
