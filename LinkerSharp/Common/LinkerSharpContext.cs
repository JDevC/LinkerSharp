using LinkerSharp.Common.Routing;
using System.Collections.Generic;

namespace LinkerSharp.Common
{
    /// <summary>
    /// Context class in which all "magic" happens
    /// </summary>
    public sealed class LinkerSharpContext
    {
        #region Private Properties
        /// <summary>
        /// Routing definition list. Every built route is appended here
        /// </summary>
        private List<RouteBuilder> RouteBuilders;
        #endregion

        #region Constructor
        public LinkerSharpContext()
        {
            this.RouteBuilders = new List<RouteBuilder>();
        }
        #endregion

        public void Run()
        {
            foreach (var RouteBuilder in this.RouteBuilders)
            {
                RouteBuilder.Route();
            }
        }

        #region Public Methods: Configuration
        public void AddRoute(RouteBuilder RouteBuilder)
        {
            this.RouteBuilders.Add(RouteBuilder);
        }
        #endregion
    }
}
