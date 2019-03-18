using LinkerSharp.Common.Models;

namespace LinkerSharp.Common.Endpoints.FTP.IFaces
{
    public interface IFTPOutConnector
    {
        bool SendData(string Endpoint, TransactionDTO Transaction);
    }
}
