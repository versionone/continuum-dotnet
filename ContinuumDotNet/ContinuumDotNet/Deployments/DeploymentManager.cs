using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Api;
using ContinuumDotNet.Connections;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Deployments;
using ContinuumDotNet.Utilities;
using RestSharp.Authenticators.OAuth;
using StackExchange.Redis;
using StructureMap;
using StructureMap.Building.Interception;

namespace ContinuumDotNet.Deployments
{
    public class DeploymentManager : IDeploymentManager
    {
        private Dictionary<string,string> _filters = new Dictionary<string, string>();
        private ICacheConnection _cache_connection;
        private const string SERVER_NAME = "servername";
        private const string FLIGHT_CODE = "flightcode";

        private Container _container;

        protected void InitializeContainer(ICacheConnection cacheConnection)
        {
            _container = new Container(_ =>
            {
                _.For<ICacheConnection>().Use(cacheConnection);
            });
            _cache_connection = _container.GetInstance<ICacheConnection>();
        }

        internal DeploymentManager(ICacheConnection cacheConnection)
        {
            InitializeContainer(cacheConnection);
            _cache_connection = cacheConnection;
        }

        public DeploymentManager(string connectionString)
        {
            InitializeContainer(new RedisCacheConnection(connectionString));
        }

        public IDeploymentManager WithServer(string serverName)
        {
            _filters[SERVER_NAME] = serverName;
            return this;
        }

        public IDeploymentManager WithFlightCode(string flightCode)
        {
            _filters[FLIGHT_CODE] = flightCode;
            return this;
        }

        private DeployedSite getDeployedSite(string flightCode, string product)
        {
            return getDeployedSite(CacheKeyExtensions.CreateFlightCodeProductKey(flightCode, product));
        }

        private DeployedSite getDeployedSite(string flightCodeProductKey)
        {
            var deployedSite =
    ((HashEntry[])_cache_connection.GetHash(flightCodeProductKey)).ConvertFromRedis<DeployedSite>();
            if (deployedSite != null && deployedSite.FlightCode != null)
            {
                deployedSite.SetCache(_cache_connection);
                return deployedSite;
            }
            return null;
        }

        private List<IDeployedSite> getDeployedSitesByServerName(string serverName)
        {
            var serverSites = _cache_connection.GetList(serverName.AsServerKey());
            var deployedSites = new List<IDeployedSite>();
            foreach (var site in serverSites)
            {
                var deployedSite = getDeployedSite(site);
                if (deployedSite != null)
                {
                    deployedSites.Add(deployedSite);
                }
            }
            return deployedSites;
        }

        public List<IDeployedSite> GetAll()
        {
            var deployedSites = new List<IDeployedSite>();
            if (_filters.Count == 0)
            {
                foreach (var server in GetAvailableServers())
                {
                    deployedSites.AddRange(getDeployedSitesByServerName(server));
                }
            }
            else if (_filters.ContainsKey(SERVER_NAME))
            {
                var filterServerName = _filters[SERVER_NAME];
                deployedSites.AddRange(getDeployedSitesByServerName(filterServerName));
            }
            else if (_filters.ContainsKey(FLIGHT_CODE))
            {
                var filterFlightCode = _filters[FLIGHT_CODE];
                foreach (var product in Constants.AllProductEditions)
                {
                    var deployedSite = getDeployedSite(filterFlightCode, product);
                    if (deployedSite != null)
                    {
                        deployedSites.Add(deployedSite);
                    }
                }
            }
            return deployedSites;
        }

        public List<string> GetAvailableServers()
        {
            return _cache_connection.GetList(Constants.AvailableServerKey);
        }

        public void AddServer(string serverName)
        {
            _cache_connection.PushLeft(Constants.AvailableServerKey, serverName);
        }

        public void RemoveServer(string serverName)
        {
            _cache_connection.RemoveListItem(Constants.AvailableServerKey, serverName);
        }

        public IDeployedSite CreateDeployedSite()
        {
            return DeployedSite.Create(_container);
        }
    }
}
