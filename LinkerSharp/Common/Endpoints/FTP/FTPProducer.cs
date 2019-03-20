using LinkerSharp.Common.Endpoints.FTP.Connectors;
using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Endpoints.IFaces;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LinkerSharpTests")]
namespace LinkerSharp.Common.Endpoints.FTP
{
    internal sealed class FTPProducer : BaseEndpoint, IProducer
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof(FTPProducer));

        private IFTPOutConnector Connector { get; set; }

        #region Constructors
        public FTPProducer(string Uri, LinkerSharpContext Context) : base(Context)
        {
            this.Endpoint = Uri;
            this.Connector = new DefaultFTPConnector();
        }

        public FTPProducer(string Uri, LinkerSharpContext Context, IFTPOutConnector OutConnector) : base(Context)
        {
            this.Endpoint = Uri;
            this.Connector = OutConnector ?? new DefaultFTPConnector();
        }
        #endregion

        #region Public Methods
        public bool SendMessage()
        {
            this.Success = Connector.SendData(this.Endpoint, this.Transaction);

            return this.Success;
        }
        #endregion
    }
}
