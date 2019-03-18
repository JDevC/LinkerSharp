using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.Direct
{
    internal class DIRECTProducer : DirectEndpoint, IProducer
    {
        public DIRECTProducer(string Path)
        {
            this.Endpoint = Path;

            if (!DirectQueue.ContainsKey(this.Endpoint))
            {
                DirectQueue[this.Endpoint] = new Queue<TransactionDTO>();
            }

            this.Transaction = new TransactionDTO() { ResponseMessage = new TransmissionMessageDTO() };
        }

        public bool SendMessage()
        {
            DirectQueue[this.Endpoint].Enqueue(this.Transaction);

            return true;
        }
    }
}
