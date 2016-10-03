using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Common;

namespace ContinuumDotNet.Interfaces.Deployments.Installers
{
    public interface ILifecycleInstaller : IHasFlightCode, IHasVersion
    {
        string Product { get; }

        ILifecycleInstaller WithProduct(string product);
        ILifecycleInstaller WithVersion (string version);
        ILifecycleInstaller WithInstanceName (string instance);
        ILifecycleInstaller WithLicenseFile(string licenseFilename);
    }
}
