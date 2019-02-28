using LinkerSharp.Common.Routing;
using System;
namespace LinkerSharpTests.Routing
{
    public class DummyRouteBuilder : RouteBuilder
    {
        public override void Route()
        {
            throw new NotImplementedException();
        }
    }
}
