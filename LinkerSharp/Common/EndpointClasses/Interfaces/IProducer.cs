
namespace LinkerSharp.Common.EndpointClasses.Interfaces
{
    /// <summary>
    /// Interface for producer classes to produce message outputs.
    /// </summary>
    public interface IProducer : IEndpoint
    {
        bool SendMessage();
    }
}
