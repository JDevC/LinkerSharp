using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinkerSharp.Common.Endpoints
{
    public sealed class EndpointFactory<T> where T : IEndpoint

    {
        private Dictionary<string, Type> AvailableEndpoints;

        public EndpointFactory()
        {
            this.AvailableEndpoints = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.GetInterfaces().Contains(typeof(T)))
                .ToDictionary(a => a.Name, a => a);
        }

        public T GetFrom(string Endpoint)
        {
            var Protocol = Endpoint.Substring(0, Endpoint.IndexOf("->")).ToUpper();
            var IFaceSuffix = typeof(T).Name.Replace("I", "");

            var ClassName = $"{Protocol}{IFaceSuffix}";

            if (this.AvailableEndpoints.ContainsKey(ClassName))
            {
                return (T)Activator.CreateInstance(this.AvailableEndpoints[ClassName], new object[] { Endpoint });
            }
            else
            {
                throw new KeyNotFoundException($"Consumer with name {ClassName} was not found.");
            }
        }
    }
}
