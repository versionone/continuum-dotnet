using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Connections;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Deployments;
using RestSharp.Authenticators.OAuth;
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
        private const string SERVER_LIST_PREFIX = "SVR_";
        private const string FLIGHT_CODE_PREFIX = "FC_";

        private Container _container;

        protected void InitializeContainer(ICacheConnection cacheConnection)
        {
            _container = new Container(_ =>
            {
                _.For<ICacheConnection>().Use(cacheConnection);
            });
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

        public List<IDeployedSite> GetAll()
        {
            var _deployedSites = new List<IDeployedSite>();
            if (_filters.ContainsKey(SERVER_NAME))
            {
                var filterServerName = _filters[SERVER_NAME];
                var server_sites = _cache_connection.GetList($"{SERVER_LIST_PREFIX}{filterServerName}");
                _deployedSites.AddRange((from site in server_sites where !_filters.ContainsKey(FLIGHT_CODE) || site == _filters[FLIGHT_CODE] select new DeployedSite() {FlightCode = site, ServerName = _filters[SERVER_NAME]}).Cast<IDeployedSite>());
            }
            else if (_filters.ContainsKey(FLIGHT_CODE))
            {
                var filterFlightCode = _filters[FLIGHT_CODE];
                var sites = _cache_connection.GetList($"{FLIGHT_CODE_PREFIX}{filterFlightCode}");
                _deployedSites.AddRange(sites.Select(site => site.Split(':')).Select(entry => new DeployedSite() {FlightCode = FLIGHT_CODE, ServerName = entry[0], Product = entry[1]}).Cast<IDeployedSite>());
            }
            return _deployedSites;
        }
        public IDeployedSite CreateDeployedSite()
        {
            return DeployedSite.Create(_container);
        }
    }
}
