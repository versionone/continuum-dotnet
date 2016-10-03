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
        public DateTime CreationDateInUtc { get; set; }
        public string BranchName { get; set; }
        public string ServerName { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }
        protected ICacheConnection Cache { get; set; }

        internal void SetCache(ICacheConnection cacheConnection)
        {
            Cache = cacheConnection;
        }

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

        public IDeployedSite WithBranchName(string branchName)
        {
            BranchName = branchName;
            return this;
        }

        public IDeployedSite WithCreationDateInUtc(DateTime creationDateTimeInUtc)
        {
            CreationDateInUtc = creationDateTimeInUtc;
            return this;
        }

        public IDeployedSite Deploy()
        {
            if (string.IsNullOrEmpty(FlightCode) || string.IsNullOrEmpty(Version) || string.IsNullOrEmpty(ServerName) ||
                string.IsNullOrEmpty(Product))
            {
                throw new MissingParameterException();
            }
            Cache.PushLeft(ServerName.AsServerKey(), $"{this.AsFlightCodeProductKey()}");
            Cache.UpsertHash(this.AsFlightCodeProductKey(), this);
            return this;
        }

        public IDeployedSite Uninstall()
        {
            Cache.RemoveListItem(ServerName.AsServerKey(), this.AsFlightCodeProductKey());
            Cache.RemoveHash(this.AsFlightCodeProductKey());
            return this;
        }

        internal static IDeployedSite Create(Container container)
        {
            return new DeployedSite()
            {
                CreationDateInUtc = DateTime.UtcNow, Cache = container.GetInstance<ICacheConnection>()
            };
        }    
    }
}
