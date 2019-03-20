using LinkerSharp.Common.Endpoints.FTP.Connectors;
using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LinkerSharpTests")]
namespace LinkerSharp.Common.Endpoints.FTP
{
    internal sealed class FTPConsumer : BaseEndpoint, IConsumer
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof(FTPConsumer));

        private IFTPInConnector Connector { get; set; }

        #region Constructors
        public FTPConsumer(string Uri, LinkerSharpContext Context) : base (Context)
        {
            this.Endpoint = Uri;

            this.Connector = new DefaultFTPConnector();
        }

        public FTPConsumer(string Uri, LinkerSharpContext Context, IFTPInConnector Connector = null) : base(Context)
        {
            this.Endpoint = Uri;

            this.Connector = Connector ?? new DefaultFTPConnector();
        }
        #endregion

        #region Public Methods
        public List<TransactionDTO> ReceiveMessages()
        {
            this.Transaction = new TransactionDTO();

            try
            {
                this.Success = this.Connector.GetData(this.Endpoint, out string StatusCode, out string DataResult);

                if (this.Success)
                {
                    this.Transaction = base.CreateTransaction(1, this.Endpoint, "", this.Params, DataResult);
                }
                else
                {
                    // Error handling
                    EndpointTools.SetErrorReason(this.Transaction, StatusCode, $"Message couldn't be sent: {DataResult}", "", _Logger);
                }
            }
            catch (WebException WebEx)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Incorrect endpoint (value -> {this.Endpoint}): {WebEx.Message}", WebEx.StackTrace, _Logger);
            }
            catch (Exception Ex)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Unable to get response from {this.Endpoint}: {Ex.Message}", Ex.StackTrace, _Logger);
            }

            return new List<TransactionDTO>() { this.Transaction };
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <param name="Success"></param>
        /// <param name="Data"></param>
        private bool RequestHandler(string Endpoint, out string StatusCode, out string Data)
        {
            bool Result = false;
            Data = "";

            var Request = WebRequest.Create(this.Endpoint);

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
        #endregion
    }
}
