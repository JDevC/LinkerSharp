using LinkerSharp.Common.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkerSharp.Common.EndpointClasses
{
    /// <summary>
    /// Util's class for endpoint classes
    /// </summary>
    public static class EndpointTools
    {
        /// <summary>
        /// This string stablishes a separation between the Endpoint's Protocol and URI. It's editable, under your own responsibility.
        /// </summary>
        public static string Separator { get; set; } = "->";

        /// <summary>
        /// Sets Protocol and URI fields from an incoming endpoint
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <param name="Protocol"></param>
        /// <param name="Uri"></param>
        /// <param name="Params"></param>
        public static void ResolveEndpoint(string Endpoint, out string Protocol, out string Uri, out Dictionary<string, string> Params)
        {
            Protocol = "";
            Uri = "";
            Params = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Endpoint) && !string.IsNullOrWhiteSpace(Endpoint))
            {
                var EndpointSplit = Endpoint.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

                if (EndpointSplit.Count() >= 2)
                {
                    Protocol = EndpointSplit[0].ToUpper();
                    Uri = EndpointSplit[1];
                    Params.Add("protocol", EndpointSplit[0].ToLower());

                    if (EndpointSplit.Count() == 3)
                    {
                        var KVList = EndpointSplit[2].Split('&');

                        foreach (var KVString in KVList)
                        {
                            var KV = KVString.Split('=');

                            Params.Add(KV[0], KV[1]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds an understandable Endpoint for the library
        /// </summary>
        /// <param name="Protocol">Selected protocol for a consumer/producer class.</param>
        /// <param name="Uri">Origin and/or destiny for the messages.</param>
        public static string ResolveReverseEndpoint(string Protocol, string Uri, Dictionary<string, string> Params)
        {
            var Result = $"{Protocol}{Separator}{Uri}";

            if (Params.Count > 0)
            {
                Result += Separator + string.Join("&", Params.Select(p => $"{p.Key}={p.Value}"));
            }

            return Result;
        }

        /// <summary>
        /// Sets error details in a message and writes the reason in a logger.
        /// </summary>
        /// <param name="DTO">Current transaction.</param>
        /// <param name="Code">Error code, when required.</param>
        /// <param name="Message">Error reason.</param>
        /// <param name="StackTrace">StackTrace for exceptions.</param>
        /// <param name="Logger"></param>
        public static void SetErrorReason(TransactionDTO DTO, string Code, string Message, string StackTrace, ILog Logger)
        {
            if (DTO == null) { DTO = new TransactionDTO() { RequestMessage = new TransmissionMessageDTO() }; }
            if (DTO.RequestMessage == null) { DTO.RequestMessage = new TransmissionMessageDTO(); }

            DTO.RequestMessage.Error.Code = Code;
            DTO.RequestMessage.Error.Reason = Message;
            DTO.RequestMessage.Error.StackTrace = StackTrace;
            Logger.Error(Message);
        }
    }
}
