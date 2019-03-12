using LinkerSharp.Common;
using LinkerSharpDemo.RouteBuilders;

namespace LinkerSharpDemo
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
