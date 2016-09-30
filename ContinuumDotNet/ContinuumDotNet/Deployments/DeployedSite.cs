using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Deployments;
using StructureMap;

namespace ContinuumDotNet.Deployments
{
    public class DeployedSite : IDeployedSite
    {
        public string FlightCode { get; set; }
        public DateTime CreationDateInUtc { get; }
        public string BranchName { get; set; }
        public string ServerName { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }
        protected ICacheConnection Cache { get; set; }

        public IDeployedSite WithVersion(string versionName)
        {
            Version = versionName;
            return this;
        }

        public IDeployedSite WithProduct(string product)
        {
            Product = product;
            return this;
        }

        public IDeployedSite WithFlightCode(string flightCode)
        {
            FlightCode = flightCode;
            return this;
        }

        public IDeployedSite WithServerName(string serverName)
        {
            ServerName = serverName;
            return this;
        }

        public IDeployedSite Deploy()
        {
            if (string.IsNullOrEmpty(FlightCode) || string.IsNullOrEmpty(Version) || string.IsNullOrEmpty(ServerName) ||
                string.IsNullOrEmpty(Product))
            {
                throw new MissingParameterException();
            }
            Cache.PushLeft(FlightCode.AsFlightCodeKey(), $"{ServerName}:{Product}");
            Cache.PushLeft(ServerName.AsServerKey(), $"{FlightCode}");
            Cache.UpsertHash(FlightCode.AsSiteKey(), "Product", Product);
            Cache.UpsertHash(FlightCode.AsSiteKey(), "Version", Version);
            return this;
        }

        public IDeployedSite Uninstall()
        {
            throw new NotImplementedException();
        }

        internal static IDeployedSite Create(Container container)
        {
            return new DeployedSite() {Cache = container.GetInstance<ICacheConnection>()};
        }
    }
}
