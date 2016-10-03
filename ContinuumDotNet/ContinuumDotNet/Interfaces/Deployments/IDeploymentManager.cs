using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Interfaces.Deployments
{
    public interface IDeploymentManager
    {
        IDeploymentManager WithServer(string serverName);
        IDeploymentManager WithFlightCode(string serverName);
        List<IDeployedSite> GetAll();
        List<string> GetAvailableServers();
        void AddServer(string serverName);
        void RemoveServer(string serverName);
    }
}
