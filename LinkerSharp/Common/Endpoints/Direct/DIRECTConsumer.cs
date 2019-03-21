using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.Direct
{
    internal sealed class DIRECTConsumer : BaseEndpoint, IConsumer
    {
        public DIRECTConsumer(string Path, LinkerSharpContext Context) : base (Context)
        {
            this.Endpoint = Path;

            if (!base.Context.DirectQueues.ContainsKey(this.Endpoint))
            {
                base.Context.DirectQueues[this.Endpoint] = new Queue<TransactionDTO>();
            }
        }

        public List<TransactionDTO> ReceiveMessages()
        {
            return base.Context.DirectQueues.ContainsKey(this.Endpoint) && base.Context.DirectQueues[this.Endpoint].Count > 0 ?
                new List<TransactionDTO>(base.Context.DirectQueues[this.Endpoint]) :
                new List<TransactionDTO>();
        }
    }
}
