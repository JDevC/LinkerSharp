using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Models;
using System.Net;

namespace LinkerSharpTests.Endpoints.FTP
{
    public class FTPConnectorMock : IFTPInConnector, IFTPOutConnector
    {
        public enum Behaviour
        {
            SUCCESS,
            NO_SUCCESS,
            WEB_EXCEPTION
        }

        private readonly Behaviour _Behaviour;

        public FTPConnectorMock(Behaviour Behaviour)
        {
            this._Behaviour = Behaviour;
        }

        public bool GetData(string Endpoint, out string StatusCode, out string Data)
        {
            StatusCode = "Success";
            Data = "Data from FTP Mock.";

            switch (this._Behaviour)
            {
                case Behaviour.SUCCESS:
                    StatusCode = "Success";
                    Data = "Data from FTP Mock.";
                    return true;
                case Behaviour.NO_SUCCESS:
                    StatusCode = "Error";
                    Data = "Error reason from FTP Mock.";
                    return false;
                case Behaviour.WEB_EXCEPTION:
                    throw new WebException("Web exception from FTP Mock");
                default:
                    return false;
            }
        }

        public bool SendData(string Endpoint, TransactionDTO Transaction)
        {
            switch (this._Behaviour)
            {
                case Behaviour.SUCCESS:
                    return true;
                default:
                    return false;
            }
        }
    }
}
