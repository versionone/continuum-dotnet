using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
