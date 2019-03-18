using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints
{
    public abstract class BaseEndpoint : IEndpoint
    {
        #region Private Fields
        private string _Endpoint = "";
        private string _Protocol = "";
        private Dictionary<string, string> _Params;
        #endregion

        #region Properties
        public bool Success { get; protected set; }
        #endregion

        #region Public Properties
        public string Protocol => this._Protocol;

        public string Endpoint
        {
            get => this._Endpoint;
            set => EndpointTools.ResolveEndpoint(value, out this._Protocol, out this._Endpoint, out this._Params);
        }

        public Dictionary<string, string> Params => this._Params;

        public TransactionDTO Transaction { get; set; }
        #endregion

        #region Protected Methods: Helpers
        protected TransportTypeEnum GetTransactionEnum(Dictionary<string, string> Params)
        {
            if (Params.ContainsKey("just-in") && bool.TryParse(Params["just-in"], out bool Val) && Val)
            {
                return TransportTypeEnum.JUST_IN;
            }
            else
            {
                return TransportTypeEnum.IN_OUT;
            }
        }

        protected TransactionDTO CreateTransaction(int ID, string Origin, string FileName, Dictionary<string, string> Params, string Content)
        {
            var Result = new TransactionDTO()
            {
                TransactionID = ID,
                Transport = this.GetTransactionEnum(Params),
                Headers = Params,
                RequestMessage = new TransmissionMessageDTO() { Origin = Origin, Name = FileName, Content = Content }
            };

            // Checks if the transactions is made from a From() or a .Enrich() method
            if (Result.Transport == TransportTypeEnum.IN_OUT)
            {
                Result.ResponseMessage = Result.RequestMessage;
            }

            return Result;
        }
        #endregion
    }
}
