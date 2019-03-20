using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints
{
    public abstract class BaseEndpoint : IEndpoint
    {
        #region Fields
        #region Private
        private string _Endpoint = "";
        private string _Protocol = "";
        private Dictionary<string, object> _Params;
        #endregion
        // Protected
        protected LinkerSharpContext Context;
        #endregion

        #region Properties
        #region Public
        public string Protocol => this._Protocol;

        public string Endpoint
        {
            get => this._Endpoint;
            set => EndpointTools.ResolveEndpoint(value, out this._Protocol, out this._Endpoint, out this._Params);
        }

        public Dictionary<string, object> Params => this._Params;

        public TransactionDTO Transaction { get; set; }
        #endregion

        public bool Success { get; protected set; }
        #endregion

        #region Constructors
        protected BaseEndpoint(LinkerSharpContext Context)
        {
            this.Context = Context;
        }
        #endregion

        #region Protected Methods: Helpers
        protected TransportTypeEnum GetTransactionEnum(Dictionary<string, object> Params)
        {
            if (Params.ContainsKey("just-in") && bool.TryParse(Params["just-in"].ToString(), out bool Val) && Val)
            {
                return TransportTypeEnum.JUST_IN;
            }
            else
            {
                return TransportTypeEnum.IN_OUT;
            }
        }

        protected TransactionDTO CreateTransaction(int ID, string Origin, string FileName, Dictionary<string, object> Params, string Content)
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
