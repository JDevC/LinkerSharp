using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.Direct
{
    internal sealed class DIRECTProducer : BaseEndpoint, IProducer
    {
        public DIRECTProducer(string Path, LinkerSharpContext Context) : base (Context)
        {
            this.Endpoint = Path;

            if (!base.Context.DirectQueues.ContainsKey(this.Endpoint))
            {
                base.Context.DirectQueues[this.Endpoint] = new Queue<TransactionDTO>();
            }

            this.Transaction = new TransactionDTO() { ResponseMessage = new TransmissionMessageDTO() };
        }

        public bool SendMessage()
        {
            base.Context.DirectQueues[this.Endpoint].Enqueue(this.Transaction);

            return true;
        }
    }
}
