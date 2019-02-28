
namespace LinkerSharp.Common.Models
{
    public sealed class TransmissionMessageDTO
    {
        /// <summary>
        /// Message complete origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Message complete destiny
        /// </summary>
        public string Destiny { get; set; }

        /// <summary>
        /// Message name (in case of files)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Message content as a string
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Field for errors at message sending/receiving
        /// </summary>
        public TransmissionMessageErrorDTO Error { get; set; }

        public TransmissionMessageDTO()
        {
            this.Error = new TransmissionMessageErrorDTO();
        }
    }

    public sealed class TransmissionMessageErrorDTO
    {
        public string Code { get; set; }

        public string Reason { get; set; }

        public string StackTrace { get; set; }
    }
}
