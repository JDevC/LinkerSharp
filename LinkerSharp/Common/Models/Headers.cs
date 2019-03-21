
namespace LinkerSharp.TransactionHeaders
{
    /// <summary>
    /// Header descriptors for all components.
    /// </summary>
    public static partial class Headers
    {
        /// <summary>
        /// Common header (consumers only). Specifies if the consumer will populate the complete transaction (request and response) or just the request.
        /// </summary>
        public const string PROTOCOL = "protocol";

        /// <summary>
        /// Common header (consumers only). Specifies a time frame in milliseconds in which the consumer will start the route again.
        /// </summary>
        public const string DELAY = "delay";

        /// <summary>
        /// Common header (consumers only). Specifies if the consumer will populate the complete transaction (request and response) or just the request.
        /// </summary>
        public const string JUST_IN = "just-in";
    }
}
