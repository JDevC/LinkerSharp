using LinkerSharp.Common.EndpointClasses.Interfaces;
using LinkerSharp.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace LinkerSharp.Common.EndpointClasses.Consumers
{
    internal class FTPConsumer : BaseEndpoint, IConsumer
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof(FTPConsumer));

        public FTPConsumer(string Uri)
        {
            this.Endpoint = Uri;
        }

        #region Public Methods
        public List<TransactionDTO> ReceiveMessages()
        {
            this.Get();

            return new List<TransactionDTO>() { this.Transaction };
        }
        #endregion

        #region Private Methods
        private async void Get()
        {
            using (var Cli = new HttpClient())
            {
                this.Transaction = new TransactionDTO()
                {
                    TransactionID = 1,
                    Transport = base.GetTransactionEnum(this.Params),
                    Headers = this.Params,
                    RequestMessage = new TransmissionMessageDTO() { Origin = this.Endpoint }
                };

                try
                {
                    using (HttpResponseMessage Response = await Cli.GetAsync(this.Endpoint))
                    {
                        this.Success = Response.IsSuccessStatusCode;

                        if (this.Success)
                        {
                            this.Transaction.RequestMessage.Content = await Response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            // Error handling
                            EndpointTools.SetErrorReason(this.Transaction, Response.StatusCode.ToString(), $"Message couldn't be sent: {Response.ReasonPhrase}", "", _Logger);
                        }
                    }
                }
                catch (ArgumentNullException ArgEx)
                {
                    EndpointTools.SetErrorReason(this.Transaction, "", $"Incorrect endpoint (value -> {this.Endpoint}): {ArgEx.Message}", ArgEx.StackTrace, _Logger);
                }
                catch (HttpRequestException HttpEx)
                {
                    EndpointTools.SetErrorReason(this.Transaction, "", $"Unable to get response from {this.Endpoint}: {HttpEx.Message}", HttpEx.StackTrace, _Logger);
                }
            }
        }
        #endregion
    }
}
