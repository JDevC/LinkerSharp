using LinkerSharp.Common.EndpointClasses.Interfaces;
using System.IO;
using System.Net;

namespace LinkerSharp.Common.EndpointClasses.Consumers.FTP
{
    public class DefaultFTPConnector : IFTPConnector
    {
        public bool GetData(string Endpoint, out string StatusCode, out string Data)
        {
            bool Result = false;
            Data = "";

            var Request = WebRequest.Create(Endpoint);

            using (var Response = Request.GetResponse() as HttpWebResponse)
            {
                StatusCode = Response.StatusCode.ToString();
                Result = Response.StatusCode.Equals(HttpStatusCode.OK);

                if (Result)
                {
                    using (var DataStream = Response.GetResponseStream())
                    {
                        using (var DataReader = new StreamReader(DataStream))
                        {
                            Data = DataReader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    Data = Response.StatusDescription;
                }
            }

            return Result;
        }
    }
}
