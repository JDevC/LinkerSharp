using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;
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

        public bool GetData(string Endpoint, Dictionary<string, object> Params, out string StatusCode, out List<TransmissionMessageDTO> Data)
        {
            StatusCode = "Success";
            Data = new List<TransmissionMessageDTO>();

            switch (this._Behaviour)
            {
                case Behaviour.SUCCESS:
                    StatusCode = "Success";
                    Data.Add(new TransmissionMessageDTO() { Content = "Data from FTP Mock." });
                    return true;
                case Behaviour.NO_SUCCESS:
                    StatusCode = "Error";
                    Data.Add(new TransmissionMessageDTO() { Error = new TransmissionMessageErrorDTO() { Code = "Error", Reason = "Error reason from FTP Mock." } });
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
