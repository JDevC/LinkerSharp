using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace LinkerSharp.Common.Endpoints.File
{
    internal sealed class FILEProducer : BaseEndpoint, IProducer
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof(FILEProducer));

        #region Private Attributes: Constants
        private const string AutoCleanParamKey = "autoclean";
        private const string AutoCleanParamDefault = "true";
        #endregion

        public FILEProducer(string Path)
        {
            this.Endpoint = Path;
            if (!this.Params.ContainsKey(AutoCleanParamKey))
            {
                this.Params[AutoCleanParamKey] = AutoCleanParamDefault;
            }

            this.Transaction = new TransactionDTO() { ResponseMessage = new TransmissionMessageDTO() };
        }

        public bool SendMessage()
        {
            if (!this.Transaction.Headers.ContainsKey(AutoCleanParamKey))
            {
                this.Transaction.Headers[AutoCleanParamKey] = this.Params[AutoCleanParamKey];
            }

            this.Transaction.ResponseMessage.Destiny = Path.Combine(this.Endpoint, this.Transaction.ResponseMessage.Name);

            try
            {
                using (var OutputFile = new StreamWriter(this.Transaction.ResponseMessage.Destiny))
                {
                    OutputFile.WriteLine(this.Transaction.ResponseMessage.Content);
                }
            }
            catch (DirectoryNotFoundException DNotFoundEx)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Endpoint not found: {DNotFoundEx.Message}", DNotFoundEx.StackTrace, _Logger);
            }
            catch (ArgumentException ArgEx)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Incorrect endpoint (value -> {this.Endpoint}): {ArgEx.Message}", ArgEx.StackTrace, _Logger);
            }
            catch (UnauthorizedAccessException NotAllowedEx)
            {
                EndpointTools.SetErrorReason(this.Transaction, "", $"Endpoint '{this.Endpoint}' cannot be reached: {NotAllowedEx.Message}", NotAllowedEx.StackTrace, _Logger);
            }

            this.Success = System.IO.File.Exists(this.Transaction.ResponseMessage.Destiny);

            this.CleanFilesAfterProcessing(this.Success, this.Transaction.Headers);

            return this.Success;
        }

        #region Private Methods
        private void CleanFilesAfterProcessing(bool Success, Dictionary<string, string> Properties)
        {
            if (Success && (!Properties.ContainsKey("autoclean") || !bool.TryParse(Properties["autoclean"], out bool Result) || Result))
            {
                System.IO.File.Delete(this.Transaction.ResponseMessage.Origin);
            }
        }
        #endregion
    }
}
