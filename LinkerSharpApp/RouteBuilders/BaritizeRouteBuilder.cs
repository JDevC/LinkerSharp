using LinkerSharp.Common.Routing;
using System;
using System.IO;

namespace LinkerSharpDemo.RouteBuilders
{
    public class BaritizeRouteBuilder : RouteBuilder
    { 
        public override void Route()
        {
            var FilePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\netcoreapp2.1", "AppFiles");

            From($"file->{Path.Combine(FilePath, "Origin")}->autoclean=false")
                .SetBody("bar", true)
                .Enrich($"file->{Path.Combine(FilePath, @"Enrichment\TestEnrichmentFile.txt")}")
                .Process(z => {
                    var Now = DateTime.UtcNow.Ticks;

                    z.ResponseMessage.Content += $" {z.RequestMessage.Content}";
                    z.ResponseMessage.Name = $"TrialFile_{Now}";
                })
                //.To($"file->{Path.Combine(FilePath, "Destiny")}");
                .To($"direct->baritize-node");
        }
    }
}
