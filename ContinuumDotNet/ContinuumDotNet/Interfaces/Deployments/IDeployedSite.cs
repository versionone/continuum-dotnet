using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Common;

namespace ContinuumDotNet.Interfaces.Deployments
{
    public interface IDeployedSite : IHasFlightCode, IHasCreationDate, IHasBranch
    {
        string ServerName { get; set; }
        string Product { get; set; }
        string Version { get; set; }
        IDeployedSite WithVersion(string versionName);
        IDeployedSite WithProduct(string product);
        IDeployedSite WithFlightCode(string flightCode);
        IDeployedSite WithServerName(string serverName);
        IDeployedSite Deploy();
        IDeployedSite Uninstall();
    }
}
