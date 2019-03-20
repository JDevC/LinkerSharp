using System.Collections.Generic;

namespace LinkerSharp.Common.Models
{
    public enum TransportTypeEnum { IN_OUT, JUST_IN }

    public sealed class TransactionDTO
    {
        public int TransactionID { get; set; }
        public TransportTypeEnum Transport { get; set; }
        public Dictionary<string, object> Headers { get; set; }

        public TransmissionMessageDTO RequestMessage { get; set; }
        public TransmissionMessageDTO ResponseMessage { get; set; }

        public TransactionDTO()
        {
            this.Headers = new Dictionary<string, object>();
            this.Transport = TransportTypeEnum.IN_OUT;
        }
    }
}
