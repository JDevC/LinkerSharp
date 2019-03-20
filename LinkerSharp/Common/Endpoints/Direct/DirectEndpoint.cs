using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.Direct
{
    public abstract class DirectEndpoint : BaseEndpoint
    {
        private static Dictionary<string, Queue<TransactionDTO>> _DirectQueue;

        protected Dictionary<string, Queue<TransactionDTO>> DirectQueue
        {
            get
            {
                if (_DirectQueue == null)
                {
                    _DirectQueue = new Dictionary<string, Queue<TransactionDTO>>();
                }

                return _DirectQueue;
            }
            set { _DirectQueue = value; }
        }

        protected DirectEndpoint(LinkerSharpContext Context) : base (Context) { }
    }
}
