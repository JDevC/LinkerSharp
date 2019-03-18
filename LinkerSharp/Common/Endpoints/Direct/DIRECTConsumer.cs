using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.Direct
{
    internal class DIRECTConsumer : DirectEndpoint, IConsumer
    {
        public DIRECTConsumer(string Path)
        {
            this.Endpoint = Path;

            if (!DirectQueue.ContainsKey(this.Endpoint))
            {
                DirectQueue[this.Endpoint] = new Queue<TransactionDTO>();
            }
        }

        public List<TransactionDTO> ReceiveMessages()
        {
            var Result = new List<TransactionDTO>();

            if (DirectQueue.ContainsKey(this.Endpoint) && DirectQueue[this.Endpoint].Count > 0)
            {
                Result = new List<TransactionDTO>(DirectQueue[this.Endpoint]);
            }

            return Result;
        }
    }
}
