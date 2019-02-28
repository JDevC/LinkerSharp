using LinkerSharp.Common;
using LinkerSharpApp.RouteBuilders;

namespace LinkerSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Context instantiation
            var Context = new LinkerSharpContext();

            // Adding routes
            Context.AddRoute(new BaritizeRouteBuilder());

            // Starting process
            Context.Run();
        }
    }
}
