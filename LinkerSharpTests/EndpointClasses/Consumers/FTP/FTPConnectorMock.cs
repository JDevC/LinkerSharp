using LinkerSharp.Common.EndpointClasses.Interfaces;
using System.Net;

namespace LinkerSharpTests.EndpointClasses.Consumers.FTP
{
    public class FTPConnectorMock : IFTPConnector
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
    }
}
