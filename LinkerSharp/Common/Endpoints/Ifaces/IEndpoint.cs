using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints
{
    public interface IEndpoint
    {
        TransactionDTO Transaction { get; set; }

        string Endpoint { get; set; }

        Dictionary<string, string> Params { get; }

        string Protocol { get; }

        bool Success { get; }
    }
}
