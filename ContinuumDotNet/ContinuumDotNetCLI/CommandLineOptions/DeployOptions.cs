using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ContinuumDotNetCLI.CommandLineOptions
{
    internal class CommonOptions
    {
        [Option('s', "Server name")]
        public string ServerName { get; set; }

        [Option('f', "Flight code")]
        public string FlightCode { get; set; }

        [Option('v', "Version")]
        public string Version { get; set; }

        [Option('p', "Product")]
        public string Product { get; set; }

        [Option('t', "Temporary Work Folder")]
        public string TempFolder { get; set; }

        [Option('a', "Base artifactory URL")]
        public string BaseArtifactoryUrl { get; set; }
        
        [Option('d', "Demo data repository name")]
        public string DemoDataRepositoryName { get; set; }

        [Option('l', "Lifecycle installer repository name")]
        public string LifecycleInstallerRepositoryName { get; set; }

        [Option('b', "Build support files repository name")]
        public string BuildSupportRepositoryName { get; set; }
    }

    [Verb("deploy", HelpText = "Deploy a site to a server")]
    class DeployOptions
    {
        [Option('i', "InstanceName")]
        public string InstanceName { get; set; }

        [Option('n', "License filename")]
        public string LicenseFileName { get; set; }

    }
}
