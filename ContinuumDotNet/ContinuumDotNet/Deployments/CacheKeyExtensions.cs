using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Deployments;

namespace ContinuumDotNet.Deployments
{
    public static class CacheKeyExtensions
    {
        public static string AsFlightCodeKey(this String flightCode)
        {
            return string.Format($"FC_{flightCode}");
        }

        public static string AsServerKey(this String serverName)
        {
            return string.Format($"SVR_{serverName}");
        }

        public static string AsSiteKey(this String flightCode)
        {
            return string.Format($"SITE_{flightCode}");
        }

        public static string AsFlightCodeProductKey (this IDeployedSite deployedSite)
        {
            return $"{deployedSite.FlightCode}_{deployedSite.Product}";
        }

        public static string CreateFlightCodeProductKey(string flightCode, string product)
        {
            var deployedSite = new DeployedSite()
                .WithFlightCode(flightCode)
                .WithProduct(product);
            return deployedSite.AsFlightCodeProductKey();
        }
    }
}
