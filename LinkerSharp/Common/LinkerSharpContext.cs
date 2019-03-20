using LinkerSharp.Common.Models;
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
        private readonly List<RouteBuilder> RouteBuilders;

        /// <summary>
        /// Queue's dict for Direct endpoint's consumption and production.
        /// </summary>
        internal readonly Dictionary<string, Queue<TransactionDTO>> DirectQueues;
        #endregion

        #region Constructor
        public LinkerSharpContext()
        {
            this.RouteBuilders = new List<RouteBuilder>();
            this.DirectQueues = new Dictionary<string, Queue<TransactionDTO>>();
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
            RouteBuilder._Context = this;
            this.RouteBuilders.Add(RouteBuilder);
        }
        #endregion
    }
}
