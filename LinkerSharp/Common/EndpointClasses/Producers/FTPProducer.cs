using LinkerSharp.Common.EndpointClasses.Interfaces;
using System.Collections.Generic;
using System.Net.Http;

namespace LinkerSharp.Common.EndpointClasses.Producers
{
    //public class FTPProducer : BaseProducer
    internal class FTPProducer : BaseEndpoint, IProducer
    {
        public FTPProducer(string Uri)
        {
            this.Endpoint = Uri;
        }

        #region Public Methods
        //public override bool SendMessage()
        public bool SendMessage()
        {
            this.Send();

            return this.Success;
        }
        #endregion

        #region Private Methods
        private async void Send()
        {
            var Content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "ddd", "foo" } });

            using (var Cli = new HttpClient())
            {
                using (HttpResponseMessage Response = await Cli.PostAsync(this.Endpoint, Content))
                {
                    this.Success = Response.IsSuccessStatusCode;
                }
            }
        }
        #endregion
    }
}
