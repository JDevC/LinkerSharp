using LinkerSharp.Common.Routing;

namespace LinkerSharpApp.RouteBuilders
{
    public class BaritizeRouteBuilder : RouteBuilder
    { 
        public override void Route()
        {
            From(@"file->C:\Users\jcarlos.garcia.ext\SelfWorks\C#\LinkerSharp\LinkerSharpTests\TestFiles\Origin->autoclean=false")
                .Process(z => {
                    var foo = "bar";

                    z.ResponseMessage.Content = foo;
                })
                .To(@"file->C:\Users\jcarlos.garcia.ext\SelfWorks\C#\LinkerSharp\LinkerSharpTests\TestFiles\Destiny\");
        }
    }
}
