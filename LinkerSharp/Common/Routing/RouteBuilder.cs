using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;

namespace LinkerSharp.Common.Routing
{
    public abstract class RouteBuilder
    {
        /// <summary>
        /// This method is intended to execute the complete messaging route.
        /// </summary>
        public abstract void Route();

        /// <summary>
        /// Starting point for the messaging route
        /// </summary>
        /// <param name="Uri">A complete URI with LinkerSharp syntax.</param>
        /// <returns></returns>
        public RouteDefinition From(string Uri)
        {
            var ConsumerFactory = new EndpointFactory<IConsumer>();

            var Consumer = ConsumerFactory.GetFrom(Uri);

            return new RouteDefinition(Consumer.ReceiveMessages());
        }
    }
}
