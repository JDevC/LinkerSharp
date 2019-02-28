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
        private List<TransactionDTO> OriginTransactions { get; set; }

        public RouteDefinition(List<TransactionDTO> OriginTransactions)
        {
            this.OriginTransactions = OriginTransactions;
        }

        /// <summary>
        /// Accepts a delegate function in order to manipulate all transactions in a concrete way.
        /// </summary>
        /// <param name="Processor"></param>
        /// <returns></returns>
        public RouteDefinition Process(Processor Processor)
        {
            foreach (var Transaction in this.OriginTransactions)
            {
                Processor(Transaction);
            }

            return this;
        }
        public delegate void Processor(TransactionDTO Transaction);

        public void To(string Uri)
        {
            var ProducerFactory = new EndpointFactory<IProducer>();

            var Producer = ProducerFactory.GetFrom(Uri);

            foreach (var Transaction in this.OriginTransactions)
            {
                Producer.Transaction = Transaction;

                Producer.SendMessage();
            }
        }
    }
}
