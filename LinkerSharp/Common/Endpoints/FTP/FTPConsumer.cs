using LinkerSharp.Common.Endpoints.FTP.Connectors;
using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

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

            var Result = new List<TransactionDTO>();
            try
            {
                this.Success = this.Connector.GetData(this.Endpoint, this.Params, out string StatusCode, out List<TransmissionMessageDTO> DataResult);

                if (this.Success)
                {
                    for (int i = 0; i < DataResult.Count; i++)
                    {
                        Result.Add(base.CreateTransaction(i + 1, this.Endpoint, DataResult[i].Name, this.Params, DataResult[i].Content));
                    }
                }
                else
                {
                    Result.AddRange(this.HandleErrorMessages(DataResult, StatusCode));
                }
            }
            catch (WebException WebEx)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Incorrect endpoint (value -> {this.Endpoint}): {WebEx.Message}", WebEx.StackTrace, _Logger);
                Result.Add(this.Transaction);
            }
            catch (Exception Ex)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Unable to get response from {this.Endpoint}: {Ex.Message}", Ex.StackTrace, _Logger);
                Result.Add(this.Transaction);
            }

            return Result;
        }
        #endregion

        #region Private Methods
        private List<TransactionDTO> HandleErrorMessages(List<TransmissionMessageDTO> Messages, string StatusCode)
        {
            var ErrorMessages = Messages.Where(x => x.Error != null && !string.IsNullOrEmpty(x.Error.Reason) && !string.IsNullOrWhiteSpace(x.Error.Reason));

            var Result = new List<TransactionDTO>();

            foreach (var ErrorMsg in ErrorMessages)
            {
                var ErrorTransaction = new TransactionDTO() { RequestMessage = ErrorMsg, ResponseMessage = ErrorMsg };

                EndpointTools.SetErrorReason(ErrorTransaction, StatusCode, $"Message couldn't be sent: {ErrorMsg.Error.Reason}", "", _Logger);
                Result.Add(ErrorTransaction);
            }

            return Result;
        }
        #endregion
    }
}
