using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.IFaces
{
    /// <summary>
    /// Interface for consumer classes to produce message inputs.
    /// </summary>
    public interface IConsumer : IEndpoint
    {
        #region Method Signatures
        List<TransactionDTO> ReceiveMessages();
        #endregion
    }
}
