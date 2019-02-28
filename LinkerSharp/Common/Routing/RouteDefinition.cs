using LinkerSharp.Common.EndpointClasses;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using LinkerSharp.Common.Models;
using System.Collections.Generic;

namespace LinkerSharp.Common.Routing
{
    public class RouteDefinition
    {
        /// <summary>
        /// Transaction list for each route
        /// </summary>
        private List<TransactionDTO> Transactions { get; set; }

        public RouteDefinition(List<TransactionDTO> Transactions)
        {
            this.Transactions = Transactions;
        }

        /// <summary>
        /// Delegate for defining a process behaviour on the fly.
        /// </summary>
        /// <param name="Transaction"></param>
        public delegate void Processor(TransactionDTO Transaction);

        /// <summary>
        /// Accepts a delegate function in order to manipulate all transactions in a concrete way.
        /// </summary>
        /// <param name="Processor"><see cref="Processor"/> function for defining a process behaviour on the fly.</param>
        /// <returns></returns>
        public RouteDefinition Process(Processor Processor)
        {
            foreach (var Transaction in this.Transactions)
            {
                Processor(Transaction);
            }

            return this;
        }

        public void To(string Uri)
        {
            var ProducerFactory = new EndpointFactory<IProducer>();

            var Producer = ProducerFactory.GetFrom(Uri);

            foreach (var Transaction in this.Transactions)
            {
                Producer.Transaction = Transaction;

                Producer.SendMessage();
            }
        }

        #region Public Methods: Helpers
        /// <summary>
        /// Direct access to edit destination message in a single-line method.
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
