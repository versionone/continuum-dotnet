using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Deployments.Installers;

namespace ContinuumDotNet.Deployments.Installers
{
    public class LifecycleInstaller : ILifecycleInstaller
    {
        private string _version;
        private string _instance;
        public string FlightCode { get; set; }

        public string Version
        {
            get { return _version; }
        }
        public string Instance
        {
            get { return _instance; }
        }
        public string Product { get; }
        public ILifecycleInstaller WithProduct(string product)
        {
            throw new NotImplementedException();
        }

        public ILifecycleInstaller WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public ILifecycleInstaller WithInstanceName(string instance)
        {
            _instance = instance;
            return this;
        }

        public ILifecycleInstaller WithLicenseFile(string licenseFilename)
        {
            throw new NotImplementedException();
        }
    }
}
