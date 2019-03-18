using LinkerSharp.Common.Routing;
using System;
using System.IO;

namespace LinkerSharpDemo.RouteBuilders
{
    public class SenderRouteBuilder : RouteBuilder
    {
        public override void Route()
        {
            var FilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\netcoreapp2.1", "AppFiles");

            From("direct->baritize-node")
                .SetBody("Canalized by direct route.", true)
                .To($"file->{Path.Combine(FilePath, "Destiny")}");
        }
    }
}
