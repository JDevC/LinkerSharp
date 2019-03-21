using LinkerSharp.Common.Endpoints;
using LinkerSharp.Common.Endpoints.IFaces;
using LinkerSharp.Common.Models;
using LinkerSharp.TransactionHeaders;
using System.Collections.Generic;
using System.Linq;

namespace LinkerSharp.Common.Routing
{
    public class RouteDefinition
    {
        /// <summary>
        /// Transaction list for each route
        /// </summary>
        private List<TransactionDTO> Transactions { get; set; }

        /// <summary>
        /// The context which it belongs.
        /// </summary>
        private LinkerSharpContext Context { get; set; }

        public RouteDefinition(List<TransactionDTO> Transactions, LinkerSharpContext Context)
        {
            this.Transactions = Transactions;
            this.Context = Context;
        }

        #region Public Methods: Routing
        /// <summary>
        /// Delegate for defining a process behaviour on the fly.
        /// </summary>
        /// <param name="Transaction"></param>
        public delegate void Processor(TransactionDTO Transaction);

        /// <summary>
        /// Accepts a delegate function in order to manipulate all transactions in a concrete way.
        /// </summary>
        /// <param name="ProcessorFunc"><see cref="Processor"/> function for defining a process behaviour on the fly.</param>
        /// <returns></returns>
        public RouteDefinition Process(Processor ProcessorFunc)
        {
            foreach (var Transaction in this.Transactions)
            {
                ProcessorFunc(Transaction);
            }

            return this;
        }

        /// <summary>
        /// Gets info from another endpoint and stores it into the transaction's request message
        /// </summary>
        /// <param name="Uri">A complete URI with LinkerSharp syntax.</param>
        /// <returns></returns>
        public RouteDefinition Enrich(string Uri)
        {
            var ConsumerFactory = new EndpointFactory<IConsumer>();

            var Consumer = ConsumerFactory.GetFrom(Uri, Context);
            Consumer.Params[Headers.JUST_IN] = "true";

            var Result = Consumer.ReceiveMessages().FirstOrDefault();
            foreach (var Transaction in this.Transactions)
            {
                foreach (var Header in Result.Headers)
                {
                    Transaction.Headers[Header.Key] = Header.Value;
                }
                Transaction.RequestMessage = Result.RequestMessage;
            }

            return this;
        }

        public void To(string Uri)
        {
            var ProducerFactory = new EndpointFactory<IProducer>();

            var Producer = ProducerFactory.GetFrom(Uri, Context);

            foreach (var Transaction in this.Transactions)
            {
                Producer.Transaction = Transaction;

                Producer.SendMessage();
            }
        }
        #endregion

        #region Public Methods: Shortcuts
        /// <summary>
        /// Direct access to edit transaction headers in a single-line method.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public RouteDefinition SetHeader(string Key, string Value)
        {
            return this.Process(x => x.Headers[Key] = Value);
        }

        /// <summary>
        /// Direct access to edit destination message's content in a single-line method.
        /// </summary>
        /// <param name="NewContent">Which content we need in the message body.</param>
        /// <param name="Append">true: The new content is appended at the end of the message; false: The message body is overwrited.</param>
        /// <returns></returns>
        public RouteDefinition SetBody(string NewContent, bool Append = false)
        {
            return this.Process(x => x.ResponseMessage.Content = Append ? x.ResponseMessage.Content + NewContent : NewContent);
        }
        #endregion
    }
}
